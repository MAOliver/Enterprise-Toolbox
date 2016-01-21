param(
	[string] $password = $(throw "-password is required.")
)

Import-Module ".\New-SelfSignedCertificateEx.ps1" -DisableNameChecking

function Add-CertificateToStore
{
    param(
        [System.Security.Cryptography.X509Certificates.StoreName] $store,
        [System.Security.Cryptography.X509Certificates.StoreLocation] $location,
        [string] $certificateFile,
        [string] $password = $null
    )

    # load the certificate to a byte array
    $certificateBytes = [System.IO.File]::ReadAllBytes($certificateFile)

    # create the parameters for the x509Certificae 
    # add the password parameter if it is not null
    # add a comma before the byte array because powershell will interpret a single array parameter incorrectly
    $x509CertificateArgumentList = @(, $certificateBytes)
    if($password -ne $null)
    {
        $x509CertificateArgumentList = $x509CertificateArgumentList + $password
    }

    # create the x509 certificate and the certificate store
    $x509Certificate = New-Object System.Security.Cryptography.X509Certificates.X509Certificate2 `
        -ArgumentList $x509CertificateArgumentList

    $certificateStore = New-Object System.Security.Cryptography.X509Certificates.X509Store `
        -ArgumentList "\\$($env:COMPUTERNAME)\$store", $location

    # add the certificate to the store
    $certificateStore.Open("ReadWrite")
    $certificateStore.Add($x509Certificate)
    $certificateStore.Close()
}

function Test-Any {
    [CmdletBinding()]
    param($EvaluateCondition,
        [Parameter(ValueFromPipeline = $true)] $ObjectToTest)
    begin {
        $any = $false
    }
    process {
        if (-not $any -and (& $EvaluateCondition $ObjectToTest)) {
            $any = $true
        }
    }
    end {
        $any
    }
}

$password
	
$store = New-Object System.Security.Cryptography.X509Certificates.X509Store("My","LocalMachine")

$store.Open("ReadOnly")

if(-Not ($store.Certificates | Test-Any {($_.FriendlyName -eq "AuthSSLLocalhostSAN")})) {
	Write-Host "Creating new SSL SAN Cert for local host with friendlyName = 'AuthSSLLocalhostSAN'"
	New-SelfsignedCertificateEx -Subject "CN=localhost" -EKU "Server Authentication", "Client authentication" -KeyUsage "KeyEncipherment, DigitalSignature" -SAN "localhost" -FriendlyName "AuthSSLLocalhostSAN" -AllowSMIME -Exportable -StoreLocation "LocalMachine"
}

if(-Not ($store.Certificates | Test-Any {($_.FriendlyName -eq "AuthSTS")})) {
	Write-Host "Creating new token signing cert with friendlyName = 'AuthSTS'"
	New-SelfsignedCertificateEx -Subject "CN=sts" -EKU "Server Authentication", "Client authentication" -KeyUsage "KeyEncipherment, DigitalSignature" -SAN "localhost" -FriendlyName "AuthSTS" -AllowSMIME -Exportable -StoreLocation "LocalMachine"
}


$store.Certificates | % {

  if(($_.FriendlyName -eq "AuthSSLLocalhostSAN") -or ($_.FriendlyName -eq "AuthSTS")) {
	$friendlyName = $_.FriendlyName
	$thumbprint = $_.Thumbprint
	$mypwd = ConvertTo-SecureString -String $password -Force –AsPlainText
	
	$type = [System.Security.Cryptography.X509Certificates.X509ContentType]::pfx

	$filePath = Join-Path $PSScriptRoot "$friendlyName.pfx"

	$bytes = $_.export($type, $mypwd)
	[System.IO.File]::WriteAllBytes($filePath, $bytes)

	Write-Host "Adding certificate with friendlyName=$friendlyName to trusted store"
	
	Add-CertificateToStore `
	-store "TrustedPeople" `
	-location "LocalMachine" `
	-certificateFile $filePath `
	-password $password

	Remove-Item $filePath
  }

  if(($_.FriendlyName -eq "AuthSSLLocalhostSAN"))
  {
	$certHash = $_.Thumbprint
	
	Write-Host "Removing any sslcerts or url reservations on port 44333"
	Invoke-Expression "netsh http delete sslcert ipport=0.0.0.0:44333"
	Invoke-Expression "netsh http delete urlacl url=https://+:44333/"
	
	Write-Host "Adding the auth reservations for port 44333"
	Invoke-Expression "netsh http add urlacl url=https://+:44333/ user=Everyone"
	Invoke-Expression "netsh http add sslcert ipport=0.0.0.0:44333 certhash=$certHash appid='{214124cd-d05b-4309-9af9-9caa44b2b74a}'"
	Invoke-Expression "netsh http delete urlacl url=https://+:44333/"
  }
}


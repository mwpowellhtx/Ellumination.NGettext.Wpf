Releasing
=========

 - Update the `.nuspec` file with a new version number. TODO: update do not need 'nuspec' as long as appropriate csproj versioning etc is bumped
 - Build in release mode
 - Invoke `PM> nuget pack`; no options needed.  But it will release the debug version, so make sure your build configuration match! Say `PM> nuget pack -Prop Configuration=Release`.
 - Sign the created package, for example by
   ```
   PM> nuget sign .\NGettext.Wpf.1.1.0-alpha.nupkg -Timestamper http://timestamp.digicert.com -CertificateFingerprint 79a047643b02e7b677d5d0a962bc02ac19e63ca8
   ```
 - Verify signing with:
   ```
   PM> nuget verify -Signatures .\NGettext.Wpf.1.1.0-alpha.nupkg
   
   Verifying NGettext.Wpf.1.1.0-alpha
   C:\Git\ngettext-wpf\NGettext.Wpf\.\NGettext.Wpf.1.1.0-alpha.nupkg
   
   Signature Hash Algorithm: SHA256
   Signature type: Author
   Verifying the author primary signature with certificate: 
       Subject Name: CN=ACCURATECH ApS, O=ACCURATECH ApS, L=Holstebro, C=DK, SERIALNUMBER=27635652, OID.2.5.4.15=Private Organization, OID.1.3.6.1.4.1.311.60.2.1.3=DK
       SHA1 hash: 79A047643B02E7B677D5D0A962BC02AC19E63CA8
       SHA256 hash: A66690776D4B00270DAA40F0336E4EE8288D2B2F9F77E6B132B63D18F0F408FF
       Issued by: CN=DigiCert EV Code Signing CA (SHA2), OU=www.digicert.com, O=DigiCert Inc, C=US
       Valid from: 06-02-2018 01:00:00 to 09-02-2021 13:00:00
   
   Timestamp: 19-02-2019 12:32:52
   
   Verifying author primary signature's timestamp with timestamping service certificate: 
       Subject Name: CN=DigiCert SHA2 Timestamp Responder, O=DigiCert, C=US
       SHA1 hash: 400191475C98891DEBA104AF47091B5EB6D4CBCB
       SHA256 hash: FC834D5BFFDE31DBA5B79BF95F573F7953BCBF9156E8525163E828EB92EA8A93
       Issued by: CN=DigiCert SHA2 Assured ID Timestamping CA, OU=www.digicert.com, O=DigiCert Inc, C=US
       Valid from: 04-01-2017 01:00:00 to 18-01-2028 01:00:00
   
   
   Successfully verified package 'NGettext.Wpf.1.1.0-alpha'
   ```

We shall leave the above reports in for historical reasons. Moving forward, however, our goals included to perform NuGet package signing along these lines.

<!-- TODO: how do we go about signing nuget package with a self CA cert?
    the matter about timing validation? -->
<!-- TODO: this question put forward concerning our approach vis-a-vis NuGet signing:
    "Windows nuget.exe NU3018 RevocationStatusUnknown"
    https://github.com/NuGet/setup-nuget/issues/133 -->
<!-- TODO: possibly better positioned as a NuGet discussion (minimum), or an issue, if it is a bug:
    "Windows nuget.exe NU3018 RevocationStatusUnknown"
    https://github.com/NuGet/Home/discussions/13362 -->
<!-- TODO: also this QA discussion on the XCA github:
    "How to use supporting nuget package signing"
    https://github.com/chris2511/xca/discussions/543 -->
<!-- TODO: Package signing timestamp server NET 5+:
    "Package signing with timestamp server does not work on .NET 5.0"
    https://github.com/NuGet/Home/issues/9725 -->
- In contemporary _dotnet_, _CSharp_, our `.csproj`, we enable `<GeneratePackageOnBuild/>`, and connect internally specificied attribute, chained via `Directory.Build.props` files.

<!-- TODO: remember to obfuscate things like passwords, maybe also paths, etc... -->
- Alternately, we, here at Ellumination Technologies, have decided to self-certify based on an internal Certificate Authority. We run from the context of the repository project directory:
  ```
  D:\Path\To\Project>nuget sign src\Ellumination.NGettext.Wpf\bin\Release\Ellumination.NGettext.Wpf.1.3.0.nupkg -CertificatePath ..\Ellumination.NGettext.Wpf.pfx -CertificatePassword ********* -Timestamper http://timestamp.digicert.com

  WARNING: NU3002: The '-Timestamper' option was not provided. The signed package will not be timestamped. To learn more about this option, please visit https://docs.nuget.org/docs/reference/command-line-reference

  Signing package(s) with certificate:
    Subject Name: E=mwpowelllde@gmail.com, CN=Ellumination.NGettext.Wpf, O=Ellumination Technologies, L=Lewes, S=DE, C=US
    SHA1 hash: DDC2057F026AB8E0AD804AED20FBB930814D082D
    SHA256 hash: 4387A339196308867479B2B8F546A6F60CF840FBBD60B75A69725FC120059250
    Issued by: E=mwpowelllde@gmail.com, CN=Intermediate CA, O=Ellumination Technologies, L=Lewes, S=DE, C=US
    Valid from: 2024-03-27 8:00:00 PM to 2034-03-27 7:59:59 PM

  WARNING: NU3018: RevocationStatusUnknown: The revocation function was unable to check revocation for the certificate.

  Package(s) signed successfully.
  ```

<!-- TODO: is not working, not sure why -->
- Or:
  ```
  D:\Path\To\Project>nuget sign src\Ellumination.NGettext.Wpf\bin\Release\Ellumination.NGettext.Wpf.1.3.0.nupkg -CertificateFingerprint ddc2057f026ab8e0ad804aed20fbb930814d082d -CertificateStoreName "Trusted Publishers" -Timestamper http://timestamp.digicert.com
  ```

<!-- TODO: untested -->
- Followed by signage verification:
  ```
  D:\Path\To\Project>nuget verify -Signatures src\Ellumination.NGettext.Wpf\bin\Release\Ellumination.NGettext.Wpf.1.3.0.nupkg
  ```

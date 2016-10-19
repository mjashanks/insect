#r "./bin/debug/Insect.dll"
#r "./bin/debug/Insect.IntegrationTests.dll"
#load "Bootstrap.fs"

open Insect.Authentication
open Insect
open InsectScripts
open Bootstrap
let authService = new AuthenticationService(Bootstrap.AuthStore)

CreateUser "mike@belfast.com" "my_twofactor_code"

authService.Verify("mike@belfast.com", "mypassword", "my_twofactor_code" )

authService.Login("mike@belfast.com", "mypassword")
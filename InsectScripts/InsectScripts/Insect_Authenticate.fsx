#r "./bin/debug/Insect.dll"
#r "./bin/debug/Insect.IntegrationTests.dll"
#load "Bootstrap.fs"

open Insect.Authentication
open Insect
open InsectScripts
open Bootstrap

CreateUser "mike@belfast.com" "my_twofactor_code"

let authService = new AuthenticationService(Bootstrap.AuthStore)

authService.Register("mike@belfast.com", "mypassword", "my_twofactor_code" )

authService.Login("mike@belfast.com", "mypassword")
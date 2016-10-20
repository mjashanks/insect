#r "./bin/debug/Insect.dll"
#r "./bin/debug/Insect.IntegrationTests.dll"
#load "Bootstrap.fs"

open Insect.Authentication
open Insect
open InsectScripts
open Bootstrap
let authService = new AuthenticationService(Bootstrap.AuthStore)

CreateUser "mike2@belfast.com" "my_twofactor_code" "/verify/12367"

authService.Verify("/verify/12367", "mypassword", "my_twofactor_code" )

authService.Login("mike2@belfast.com", "mypassword")
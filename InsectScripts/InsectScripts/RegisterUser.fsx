#r "./bin/debug/Insect.dll"
#r "./bin/debug/Insect.IntegrationTests.dll"
#load "Bootstrap.fs"

open Insect.Authentication
open Insect
open InsectScripts
open Bootstrap

CreateUser "mike"

let authService = new AuthenticationService(Bootstrap.AuthStore)

authService.Register()


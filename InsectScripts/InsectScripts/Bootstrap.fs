﻿namespace InsectScripts

open Insect.Authentication
open Insect.IntegrationTests
open Insect
open Insect.Stores

module Bootstrap =

    let Config =
        let mutable config = new Config();
        config.Database <- "InsectTest"
        config.Server <- "localhost\\bluezinc"
        config

    let AuthStore = 
        new AuthStore(Config)

    let CreateUser username = 
        DbCreator.CreateUser(Config, username)



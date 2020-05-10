---
theme : "black"
transition: "zoom"
highlightTheme: "darkula"
separator: ^---$
verticalSeparator: ^--$
revealOptions:
    progress: false
    overview: false
    controls: false
---

#### .NET BA #24

## Distributed APplication Runtime

#### by Marek Linka

---

## About me

* Expert software engineer @ ERNI
* Focus on up-and-coming .NET (Core, ASP.NET, ML.NET)
* Machine learning enthusiast
* CoreFX and ML.NET contributor

----

https://github.com/mareklinka

https://twitter.com/mareklinka

---

## Agenda

* What is DAPR
* What can DAPR do for me?
* Setting up development environment for DAPR
* Using DAPR in ASP.NET Core applications
* DAPR Actors
* Going to production

---

## Microservices
* Building and operating microservices is hard
* Potential problems when creating microservice architectures:
    * Platform specificity for runtimes
    * Different infrastructure requirements for different services
    * Migrating an existing piece of software is difficult
    * Vendor lock

--

### Topics to consider
* Service discovery
* Communication protocol
* Service orchestration
* State storage
* Secret management
* Logging and tracing
* Cloud service integration
* Development and debugging
* Going to production

---

## What is DAPR?

> An event-driven, portable runtime for building microservices on cloud and edge.

--

## For real, what IS DAPR?

A runtime and a set of building blocks for building service-oriented software solutions

Also:
* Platform-agnostic
* Language-agnostic
* Cloud-ready but edge-compatible
* Open source
* Extensible

--

## DAPR architecture

![test](https://raw.githubusercontent.com/dapr/docs/master/images/overview.png)

--

## How does DAPR work?

* For each building block DAPR provides, there is a set of platform-specific components
* E.g. for state management (a.k.a. storage):
    * Redis, MongoDB, SQL Server, Azure CosmosDB etc.
* Components are picked and configured via YAML files
* You can mix, match, and swap components transparently

--

## How do you integrate DAPR with your app?

* Some languages have SDKs
    * .NET Core, Java, Go, Rust etc.
* The DAPR API can be invoked via HTTP of gRPC directly
* At runtime, DAPR runs as a "sidecar" to your application
* The runtime loads and configures your desired components and exposes them via the DAPR API

--

## The two hosting modes

* DAPR can be configured to run in two modes:
    * Standalone
        * DAPR runs as a separate process
        * This is great for development and small-scale deployments
    * Kubernetes
        * DAPR runs as a sidecar container
        * This is usefuly for large-scale deployments

--

## DAPR in standalone mode

![](https://raw.githubusercontent.com/dapr/docs/master/images/overview_standalone.png)

--

## DAPR in Kubernetes mode

![](https://raw.githubusercontent.com/dapr/docs/master/images/overview_kubernetes.png)

--

## DAPR components 1/2

* Service discovery and invocation
    * mDNS (zeroconf) or Kubernetes DNS
* State management
    * Redis, MongoDB, SQL Server, Azure CosmosDB etc.
* Pub/sub
    * Redis Streams, Kafka, Azure Service Bus, RabbitMQ etc.

--

## DAPR components 2/2

* Bindings and triggers
    * Several AWS and Azure services, HTTP, MQTT, Rabbit, Redis etc.
* Secret stores
    * AWS Secret Manager, Azure KeyVault, HashiCorp Vault etc.
* Logs, metrics, traces
    * Azure Monitor, Grafana, Elastic Search, Application Insights

---

# Setting up DAPR

--

* Documentation is available
* Requirement: Docker with linux containers
* At the end, two containers will be started:
    * Redis
    * An actor placement service
* The DAPR executable will be added to your path

--

## [Optional] Setting up VS Code

* Developing and debugging DAPR apps is much easier in VS Code
* DAPR extension is available
    * Adds support for additional tasks
    * Allows to scaffold necessary configurations
    * Shows running DAPR apps
* More configuration might be needed depending on your setup, such as multi-target debugging

--

## Demo

1. `docker ps` and `dapr --version`
2. VS Code
    * DAPR extension
    * Solution layout
    * VS Code tasks and launch configuration

---

# Using DAPR in ASP.NET Core apps

--

## Your first DAPR service

1. Scaffold your desired ASP.NET Core app (SPA, WebAPI, gRPC, doesn't matter)
2. Install DAPR nugets (currently in preview)
3. ???
4. Profit!

--

## Well, that's not doing much A.K.A. DEMO
The idea of the demo is to showcase a multi-service setup with an SPA, a user management service, and a notification service

1. DAPR-enabled SPA
2. Service invocation
3. Secret store access
4. Debugging

--

## The magic of service discovery

* Invoking a service's API controller via DAPR is the same as invoking it via HTTP...
    * ...except simpler, because you don't need to know its address
* DAPR takes care of finding the service, delivering the payload, and returning the response
* All HTTP verbs are supported
* This form of invocation is synchronous - the caller waits for a response

--

## Demo

1. Login controller
2. State persitance
3. Publishing a message

--

## Pub/sub using DAPR

* Synchronous invocation might not be desired
* Pub/sub provides asynchronous messaging capabilities
* Pub/sub messages can also invoke ASP.NET controller actions
* Delivery semantics:
    * At least once
    * If multiple isntances of the same service subscribe to a topic, only one instance of the service gets the message

--

## DEMO

1. Sending a message
2. Subscribing to a topic and reacting to a message

--

## Pluggable architecture

* Because DAPR provides a unified API for its building blocks, it's possible to replace components transparently*

----

\* If everything works as expected, see below

--

## DEMO

1. Swap Redis for SQL Server and watch it fail
2. DAPR is still pre-release and there are bugs
3. Call to action

---

# DAPR Actors

--

## Not quite like DiCaprio

> The actor model [...] is a mathematical model of concurrent computation that treats __actor__ as the universal primitive of concurrent computation. In response to a message it receives, an actor can: make local decisions, create more actors, send more messages, and determine how to respond to the next message received.

--

## That's great, but...

* An actor is a discreet (addressable) unit of data and computation
* It can have its own persistent state but cannot directly affect the state of other actors
* E.g. a bank account can be modelled as an actor:
    * The actor's address is the account number
    * The actor's state is the account balance
    * The actor responds to two messages
        * `deposit n`
        * `withdraw n`

--

## Advantages of actors 1/2

* The actor model greatly reduces complexity of concurrent systems
    * An actor is single-threaded and can only handle one message at a time
    * This makes reasoning about operations much easier

--

## Advantages of actors 2/2

* The actor model abstracts a lot of concerns away (e.g. topology)
    * The user doesn't need to know where an actor lives (or even IF it lives)
    * Actors are free to migrate, be deactivated and reactivated
    * Failover is also simplified

--

## A Disclaimer on actors

> As with everything, actors have their uses and their disadvantages. Not everything can be easily modelled as actors - and even if your particular problem can be, it's a fundamentally different way of thinking about systems so it will take some getting used to.

--

## Actors using DAPR

* DAPR has its own implementation of the actor pattern
* Actors can be invoked from other applications using the provided SDKs or direct DAPR API requests
* The actor placement service (shown previously) is responsible for distributing actor instances across the environment

--

## DEMO

1. Creating an actor interface
2. Implementing an actor (bonus: polyglot systems)
3. Invoking an actor

---

# Going to production

--

## Prepare for Kubernetes

* DAPR should make it easy to develop locally and deploy wherever you can get Kubernetes
* However, handling Kubernetes and Helm is a whole new can of worms so I decided not to talk about it here
* If there is interest, we might prepare a followup session to go into details on this side of DAPR as well
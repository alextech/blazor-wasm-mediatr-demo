# Blazor webassembly without REST api
Demonstration of blazor webassembly communicating to aspnet server using RPC-like style.

Does not require HTTP controller handler for every request. All commands and queries dispatched
by webassembly UI are [MediatR requests](https://github.com/jbogard/MediatR) that are serialized 
and sent to single entry point on server.
Server deserializes them back to same objects, so the server execution is seamless.
Development experience feels close to using blazor server.

Request and response object types are in a shared project that is compiled to both server and client.

As long as server project has a corresponding [handler](https://github.com/jbogard/MediatR/wiki#basics) for 
a request command compiled into it, communication will work.
A common MediatR pipeline behavior automatically encodes all sent requests from UI.


## Simple query example

Taking Microsoft's own weather example:
* [ForecastQuery](https://github.com/alextech/blazor-wasm-mediatr-demo/blob/master/Shared/ForecastQuery.cs) object in shared library defines intent and specifies expected return data type. 
* [FetchData.razor](https://github.com/alextech/blazor-wasm-mediatr-demo/blob/master/Client/Pages/FetchData.razor#L45) page on client creates query and sends it to mediator.
* [ForecastQueryHandler](https://github.com/alextech/blazor-wasm-mediatr-demo/blob/master/Server/Handlers/ForecastQueryHandler.cs) on server processes query returning result as type specified in query.

## Command with status example

More complex example with a response containing status details along with data is shown in 
[NewGuidCommand](https://github.com/alextech/blazor-wasm-mediatr-demo/blob/master/Shared/NewGuidCommand.cs)
and [corresponding handler](https://github.com/alextech/blazor-wasm-mediatr-demo/blob/master/Server/Handlers/IncrementCommandHandler.cs).
It implements __ICommand\<T\>__ interface to designate return type as __CommandResponse__ that has success or fail status, and 
expected return type specified by generic __\<T\>__

------

Full video walkthrough at [https://youtu.be/NPnr64DnXqw](https://youtu.be/NPnr64DnXqw).
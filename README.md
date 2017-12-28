# Friday.NET


### What is Friday.NET?

It is a framework 
 Contain lots of extension methods, and  auxiliary classes useful almost in every project.
 


#### Friday.Base
 NET Core compatible
 Xamarin compatible
 Unity compatible
 Reference free 
  
 
 
 # Routing
 
 In network applications it is often the case when the incoming data needs to be routed 
 
--- 
```c#

class BaseMessage
{
 public string SomeData { get; set; }
 public bool SomeBoolean { get; set; }    
}

class DerivedMessage : BaseMessage
{
 public string SomeDerivedData { get; set; }
}

class Processor: IMessageHandler<DerivedMessage>
{
	public void HandleMessage(DerivedMessage message)
	{
		//Some handling logic
	}
}


```
Consider Abstract message receiver (Can be websocket, byte stream e.t.c)

```c#

class Transport
	{
	 private  readonly RoutingProvider routingProvider;
	 
	 Transport()
	 {
	 	routingProvider = new RoutingProvider();
	 	var routeRule= new RouteRule(new Processor(), RouteOptions.UseInterfaceMessageHandler());
	 	routingProvider.RegisterRoute(routeRule);
	 }
     
	 		
	 public void OnMessage(string textMessage)
	 {			
	 	BaseMessage baseMessage= someDeserializer.Deserialize(textMessage);       
	 	routingProvider.RouteObject(baseMessage); //Will trigger  HandleMessage in our Processor
	 }

	}
```

You can also route methods

```c#
interface IConnectionEstablished
{
 void ProcessConnectionEstablished();
}

//RouteCall will call corresponding method in each registered route that implement IConnectionEstablished
routingProvider.RouteCall<IConnectionEstablished>(processor => processor.ProcessConnectionEstablished()); 
```

#### Handler methods can be async

```c#
class Processor : IMessageHandlerAsync<DerivedMessage>
	{
		public async Task HandleMessage(DerivedMessage message)
		{
			await Task.Delay(TimeSpan.FromSeconds(10));
			//process message
		}
	}
```




For async routing you can use Async call

```c#
await routingProvider.RouteObjectAsync(baseMessage);
```

Or use regular call

```c#
routingProvider.RouteObject(baseMessage);
```

#### Exceptions can be caught as usual

```c#
try
{
 routingProvider.RouteObject(baseMessage);
}
catch(Exception e)
{

 //...
	
}

```

Attribute method protection also could be used, Friday.Base include AuthRequiredAttribute as example
and could be used in this way


```c#


class AuthRequestDto
{
	public string UserName{get;set;}	
}

internal class AuthRequiredApiProcessor : IMessageHandler<DerivedMessage>, IContextMessageHandler<AuthRequestDto>
	{	
				
		[AuthRequired]
		public void HandleMessage(DerivedMessage message)
		{
			
		}
		
		public void HandleMessage(IRoutingContext context, AuthRequestDto message)
		{
			context.Principal = new BasicUserNamePrincipal(message.UserName);
		}
	}
```


```c#
routingProvider = new RoutingProvider();

var routeRule= new RouteRule(new AuthRequiredApiProcessor(), RouteOptions.UseInterfaceMessageHandler());

routingProvider.RegisterAttributeHandler(new AuthRequiredAttributeHandler());
routingProvider.RegisterRoute(routeRule);
```

Attempt to route message before authorization:

```c#
routingProvider.RouteObject(new DerivedMessage()); //Will generate NotAuthorizedException
```

Correct auth will looks like

```c#
routingProvider.RouteObject(new AuthRequestDto(){UserName="test"}); 
//than you can use routingProvider as usual
```



# Friday.NET

### What is Friday.NET?

Friday.NET it's a general-purpose framework for solving a variety of applied problems

Contains the following assemblies
#### Friday.Base
Lots of extension methods, Routing, and  auxiliary classes useful almost in every project.
 
 + NET Core compatible
 + Xamarin compatible
 + Unity3d compatible
 + Third Party Reference free 
  
 
 
# Routing
 
Useful in network applications with custom protocol messages, when they must be routed to specific processors

Single input, one or more outputs 
--- 


Sample classes

```c#
class BaseMessage
{
 
}

class DerivedMessage : BaseMessage
{

}

class Processor: IMessageHandler<DerivedMessage>
{
	public void HandleMessage(DerivedMessage message)
	{
		//Some handling logic
	}
}


```
Consider Abstract message receiver (Can be Websocket,  byte stream e.t.c)

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
     
	 
	 private BaseMessage DeserializeMessage(string textMessage)
	 {
		 return new DerivedMessage();
	 }
	 		
	 public void OnMessage(string textMessage)
	 {	
	 	BaseMessage baseMessage= DeserializeMessage(textMessage);       
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

Attribute method protection also could be used, Friday.Base include AuthRequiredAttribute 


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
			//message processing AFTER  client authorized
		}
		
		public void HandleMessage(IRoutingContext context, AuthRequestDto message)
		{
			context.Principal = new BasicUserNamePrincipal(message.UserName);
		}
	}
```

Register processor with attribute handler

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



# Event Invoker
---
 For example you develop a public API  library with many events
 
 ```c# 
 class BaseMessage
 {
	 
 } 
 
 class SomeMessage : BaseMessage
 {
	 
 }
 
 class SomeOtherMessage : BaseMessage
 {
	 
 }
 
 
 class MyApplicationApi
 {
	 public event EventHandler<SomeMessage> OnSomeMessage;
	 public event EventHandler<SomeOtherMessage> OnSomeOtherMessage;
 }
 
 ```
 
 Calling of each event can be painful 
 
 ```c#
 private void OnMessage(BaseMessage msg)
 {
	 if(message is out var SomeMessage sm)
		 OnSomeMessage?.Invoke(sm);
	 	 //and for each message type same code	 
 }
 
 ```
 Offcourse you can generate T4 template for that kind of stuff but...
 
 How about that? 
 ```c#
  private void OnMessage(BaseMessage msg)
 {
	 EventUtils.InvokeEvent(this,msg); 	   //will find corresponding EventHandler in 'this', and call it.
 } 
 ```
 
  
 
 
 ## Friday.Base.Extensions

 ### Enum Extensions
 ---
 
 ```c#
 
enum Numbers
		{
			
			One,
			Two,
			Three

		}
 
public void Test()
	{
		
		var testEnum = Numbers.One;
		testEnum = testEnum.Next();  //testEnum now contain Two			
		testEnum = testEnum.RandomValue(seed:222);  //testEnum now contain Three
	}
 
 ```
 
  ### Enumerable Extensions
 ---
 ```c#
  var list = new List<string> {"one", "two", "three", "four","five"};
 ```
 
 
 Take random element
 
 ```c#
  list.TakeRandom(); 
 ```
 
 
 
 Separate list to many chunks
  
 ```c#
 void ChunkifyDemo()
 {							
	var chunks=list.TakeChunks(2); 	
	//chunks now contain 2 elements
    //"one","two" 
	//"three", "four","five"	
 }  
 ```
  
 Shuffle (random order)
 
 ```c#
 list = list.Shuffle(); 
 ```
  
 
 Swap
 ```c#
 list.Swap("one", "two"); 
 //output two one three four
 ```
 
 Or by index
 ```c#
 list.Swap(0, 1); 
 //output two one three four
 ```
 
 
### Formatter Extensions
---
 
Standart separator
 
 ```c#
string debuggerOutput= list.ToPrettyString(); 

/*
one
two
three
four
five
*/
``` 


Any separator
 ```c#
string debuggerOutput= list.ToPrettyString(","); 

/*
one,two,three,four,five
*/
``` 
 
 
 Dictionary
 ```c#
 
 var dictionary = new Dictionary<int,string>()
 {
  {0,"zero" },
  {1,"one" },	 
 }
 
 
 var debuggerOutput=dictionary.ToPrettyString();
 /*
 0=zero
 1=one 
 */
 
 ```
 
 
 ### Reflection Extensions
 ---
 
 Automapper is super cool but sometimes it's too complicated for simple mapping

So meet simpliest ever mapping
 
 ```c#
 
 class Entity
 {
	 public string UserName{get;}
	 public float Field;	 
 }
 
 class SomeMessageDto
 {
	public string UserName;
	public float Field;	
 }
 
 
 [TestMethod]
 void MappingShouldSuccess()
 {
	 var entity = new Entity();
	 var dto=entity.MapPropertiesWithFieldsTo<SomeMessageDto>();
	 
	 Assert.IsTrue(entity.UserName==dto.UserName);
	 Assert.IsTrue(entity.Field==dto.Field); 
	 
 }
  
 ```
 
 Or 
 
 ```c#
SomeDto dto=new SomeDto();
entity.MapPropertiesWithFieldsTo(dto); 
 ```
 
 Or even
 
 
```c#
SomeDto dto=new SomeDto();
dto.MapPropertiesWithFieldsFrom(entity); 
```
 
   
### String Extensions
---
```c#
void StringExtensionsDemo()
{
	string s="Hello KittY, Avilo super mech terran";
	
	s.Contains("Avilo"); //true
	s.ContainsNoCase("kitty"); //true
	s.IsValidEmail(); //false	
}
```
   
 
### Struct Extensions
---
```c#

struct ExampleStruct
{
 public int A;
 public string B; 
}


void Demo()
{
	var exampleStruct= new ExampleStruct();	
	byte[] array=exampleStruct.ToByteArray(); //ready for network send		
	var restoredStruct=array.ToStructure<ExampleStruct>(); //restore it ;)	
}

``` 
 
  ### Exception Extensions
--- 
      
 ```c#
  Exception e= new Exception();   
  e.GetDeepestInnerException();  
  ```
  
  
 
 ### Reference Counter
 ---
 ```c#
 var rc = new ReferenceCounter();  
  rc.OnNoReferencesLeft += ()=> Console.WriteLine("No References left, object can be disposed");  
  rc.Acquire();
  rc.Release();
  
 ```
 
 
 ## Friday Random
  ---
  ```c#  
FridayRandom.SetSeed(seed:1234);
FridayRandom.IsRandomChanceOccured(chanceInPercent: 50); 
FridayRandom.GetNext(0,100);
FridayRandom.GetNextIncludingMax(0,100);
```
 
 
 ## And some miscellaneous stuff 
 ---
 ```c# 
 void Foo()
 {	 
	 Console.WriteLine(Uptime.FullUptime); //print App uptime
     FridayDebugger.SafeBreak();	  // Break only if debugger attached
 } 
 ```
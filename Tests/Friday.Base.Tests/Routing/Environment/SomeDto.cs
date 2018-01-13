using Friday.Base.Routing.Interfaces;

namespace Friday.Base.Tests.Routing.Environment
{
	public class SomeDto
	{
	}
	public class SomeDto2
	{
	}

	public interface ComplexInterface : IMessageHandler<SomeContext, SomeContext>, IMessageHandler<SomeContext, SomeDto>, IMessageHandler<SomeDto>, IMessageHandler<SomeDto2>, IMessageHandler<SomeDto, SomeDto>, IMessageHandler<SomeDto, SomeContext>
	{

	}


	public class SomeContext
	{


	}


}
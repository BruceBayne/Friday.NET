namespace Friday.Base.Tests.Reflection.Environment
{
	public class TestEntity
	{
		public int Id = 12345;
		public string Name { get; set; } = "EntityName";
		public bool NotExistInDto = true;
		public float f = 1.33f;
	}
}
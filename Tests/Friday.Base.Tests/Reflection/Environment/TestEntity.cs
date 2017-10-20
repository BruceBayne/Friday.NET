namespace Friday.Base.Tests.Reflection.Environment
{
    public class TestEntity
    {
        public int Id = 5;
        public string Name { get; set; } = "Dto";
        public bool NotExistInDto = true;
    }
}
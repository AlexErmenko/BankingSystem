using Xunit;

namespace xUnitLib
{
	public class Class1
	{
		private int Add(int x, int y) { return x + y; }

		[Theory]
		[InlineData(3)]
		[InlineData(5)]
		[InlineData(6)]
		public void MyFirstTheory(int value) { Assert.True(IsOdd(value)); }

		private bool IsOdd(int value) { return value % 2 == 1; }

		[Fact]
		public void FailingTest() { Assert.Equal(expected: 5, Add(x: 2, y: 2)); }

		[Fact]
		public void PassingTest() { Assert.Equal(expected: 4, Add(x: 2, y: 2)); }
	}
}
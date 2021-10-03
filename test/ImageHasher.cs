using System;
using Xunit;
using Xunit.Abstractions;

namespace LooksLikeIt.NET.Tests
{
    public class UnitTest1
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public UnitTest1(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void GetDHash_CompareOriginAndResizedImages_ShouldBeSameHashes()
        {
            var hasher = new ImageHasher();

            var hash1 = hasher.GetDHash("img/1.png");
            var hash2 = hasher.GetDHash("img/1-resized.png");
            
            Assert.Equal(hash1, hash2);
        }
        
        [Fact]
        public void GetDHash_CompareOriginAndCroppedImages_ShouldBeDifferentHashes()
        {
            var hasher = new ImageHasher();

            var hash1 = hasher.GetDHash("img/1.png");
            var hash2 = hasher.GetDHash("img/1-cropped.png");
            
            Assert.NotEqual(hash1, hash2);
        }
        
        [Fact]
        public void GetDHash_CompareTwoDifferentImages_ShouldBeDifferentHashes()
        {
            var hasher = new ImageHasher();

            var hash1 = hasher.GetDHash("img/1.png");
            var hash2 = hasher.GetDHash("img/2.jpg");
            
            Assert.NotEqual(hash1, hash2);
        }
    }
}
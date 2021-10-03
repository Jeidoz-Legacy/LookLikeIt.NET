using System;

namespace LooksLikeIt.NET
{
    public interface IImageHasher
    {
        string GetPHash(string pathToImage);
        string GetDHash(string pathToImage);
    }
}
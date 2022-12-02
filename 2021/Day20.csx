#!/usr/bin/env dotnet-script
#nullable enable

int GetNeighboorhoodPower(bool[,] image, int x, int y, bool infinityValue)
{
    var height = image.GetLength(0);
    var width = image.GetLength(1);
    var binaryPower = string.Empty;

    for (var j = -1; j <= 1; j++)
    {
        for (var i = -1; i <= 1; i++)
        {
            var ny = y + j;
            var nx = x + i;

            binaryPower += (ny < 0 || ny >= height || nx < 0 || nx >= width)
                ? infinityValue ? '1' : '0'
                : image[y + j, x + i] ? '1' : '0';
        }
    }
    return Convert.ToInt32(binaryPower, 2);
}

bool[,] FilterImage(bool[,] image, string enhancementAlgorithm, bool infinityValue = false)
{
    // To work on infinity images, we just look at the space that might be affected by
    // the input image. A 3x3 matrix will overscan by 2 rows / columns. While only the first over 
    // scanned column is affected by the input image.
    var filteredImage = new bool[image.GetLength(0) + 2, image.GetLength(1) + 2];
    for (var y = 0; y < filteredImage.GetLength(0); y++)
    {
        for (var x = 0; x < filteredImage.GetLength(1); x++)
        {
            var power = GetNeighboorhoodPower(image, x - 1, y - 1, infinityValue);
            filteredImage[y, x] = enhancementAlgorithm[power] == '#';
        }
    }
    return filteredImage;
}

static T[,] To2DArray<T>(this IEnumerable<IEnumerable<T>> source)
{
    var jaggedArray = source.Select(x => x.ToArray()).ToArray();
    var array = new T[jaggedArray.Length, jaggedArray[0].Length];

    for (var y = 0; y < jaggedArray.Length; y++)
    {
        for (var x = 0; x < jaggedArray[y].Length; x++)
        {
            array[y, x] = jaggedArray[y][x];
        }
    }
    return array;
}

var data = File.ReadAllLines("Data/Day20.txt");
var algorithm = data.First();
var image = data.Skip(2)
                .Select(y => y.Select(x => x == '#'))
                .To2DArray();

var filteredImage = FilterImage(image, algorithm);

for (var i = 0; i < 50; i++)
{
    image = FilterImage(image, algorithm, i % 2 != 0);
}

var litPixel = image.Cast<bool>()
                    .Count(p => p == true);
    
Console.WriteLine("Day 20: Trench Map");
Console.WriteLine($"{litPixel}");
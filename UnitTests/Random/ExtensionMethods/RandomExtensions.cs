﻿/*
Copyright (c) 2012 <a href="http://www.gutgames.com">James Craig</a>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.*/

using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.Random.DefaultClasses;
using Utilities.Random.ExtensionMethods;
using Utilities.Random.NameGenerators;
using Utilities.Random.StringGenerators;
using Xunit;

namespace UnitTests.Random.ExtensionMethods
{
    public class RandomExtensions
    {
        public RandomExtensions()
        {
            new System.Random().ResetGenerators();
        }

        [Fact]
        public void Next()
        {
            System.Random Random = new System.Random();
            Assert.DoesNotThrow(() => Random.Next<bool>());
            Assert.DoesNotThrow(() => Random.Next<byte>());
            Assert.DoesNotThrow(() => Random.Next<char>());
            Assert.DoesNotThrow(() => Random.Next<decimal>());
            Assert.DoesNotThrow(() => Random.Next<double>());
            Assert.DoesNotThrow(() => Random.Next<float>());
            Assert.DoesNotThrow(() => Random.Next<int>());
            Assert.DoesNotThrow(() => Random.Next<long>());
            Assert.DoesNotThrow(() => Random.Next<sbyte>());
            Assert.DoesNotThrow(() => Random.Next<short>());
            Assert.DoesNotThrow(() => Random.Next<uint>());
            Assert.DoesNotThrow(() => Random.Next<ulong>());
            Assert.DoesNotThrow(() => Random.Next<ushort>());
            Assert.DoesNotThrow(() => Random.Next<DateTime>());
            Assert.DoesNotThrow(() => Random.Next<Color>());
            Assert.DoesNotThrow(() => Random.Next<TimeSpan>());
            Assert.DoesNotThrow(() => Random.Next<string>());
        }

        [Fact]
        public void Next2()
        {
            System.Random Random = new System.Random();
            Assert.DoesNotThrow(() => Random.Next<bool>(false, true));
            Assert.InRange(Random.Next<byte>(1, 29), 1, 29);
            Assert.InRange(Random.Next<char>('a', 'z'), 'a', 'z');
            Assert.InRange(Random.Next<decimal>(1.0m, 102.1m), 1, 102.1m);
            Assert.InRange(Random.Next<double>(1.0d, 102.1d), 1, 102.1d);
            Assert.InRange(Random.Next<float>(1, 102.1f), 1, 102.1f);
            Assert.InRange(Random.Next<int>(1, 29), 1, 29);
            Assert.InRange(Random.Next<long>(1, 29), 1, 29);
            Assert.InRange(Random.Next<sbyte>(1, 29), 1, 29);
            Assert.InRange(Random.Next<short>(1, 29), 1, 29);
            Assert.InRange(Random.Next<uint>(1, 29), (uint)1, (uint)29);
            Assert.InRange(Random.Next<ulong>(1, 29), (ulong)1, (ulong)29);
            Assert.InRange(Random.Next<ushort>(1, 29), (ushort)1, (ushort)29);
            Assert.InRange(Random.Next<DateTime>(new DateTime(1900, 1, 1), new DateTime(2000, 1, 1)), new DateTime(1900, 1, 1), new DateTime(2000, 1, 1));
            Assert.Equal(10, Random.Next<string>().Length);
        }

        [Fact]
        public void NextIEnumerable()
        {
            System.Random Random = new System.Random();
            Assert.InRange(Random.Next<int>(new int[] { 1, 2, 3, 4, 5 }), 1, 5);
        }


        [Fact]
        public void NextLoremIpsumTest()
        {
            Utilities.Random.Random Rand = new Utilities.Random.Random();
            Assert.NotNull(Rand.Next<string>(new LoremIpsumGenerator(1, 4)));
        }

        [Fact]
        public void NextStringTest()
        {
            Utilities.Random.Random Rand = new Utilities.Random.Random();
            Assert.Equal(10, Rand.Next<string>(new RegexStringGenerator(10)).Length);
        }

        [Fact]
        public void NextStringTest2()
        {
            Utilities.Random.Random Rand = new Utilities.Random.Random();
            Assert.True(Regex.IsMatch(Rand.Next<string>(new PatternGenerator("(###)###-####")), @"\(\d{3}\)\d{3}-\d{4}"));
            Assert.True(Regex.IsMatch(Rand.Next<string>(new PatternGenerator("(@@@)###-####")), @"\([a-zA-Z]{3}\)\d{3}-\d{4}"));
        }

        [Fact]
        public void NextName()
        {
            Utilities.Random.Random Rand = new Utilities.Random.Random();
            Assert.Equal(3, Rand.Next<string>(new NameGenerator(false, true, true, false)).Split(' ').Length);
            Assert.Equal(2, Rand.Next<string>(new NameGenerator(false, false, true, false)).Split(' ').Length);
            Assert.Equal(4, Rand.Next<string>(new NameGenerator(true, true, true, false)).Split(' ').Length);
            Assert.Equal(5, Rand.Next<string>(new NameGenerator(true, true, true, true)).Split(' ').Length);
        }

        [Fact]
        public void RegisterGenerator()
        {
            Utilities.Random.Random Rand = new Utilities.Random.Random();
            Assert.DoesNotThrow(() => Rand.RegisterGenerator<string>(new NameGenerator()));
            Assert.True(100.Times(x => Rand.Next<string>()).All(x => x.Split(' ').Length == 2));
        }

        [Fact]
        public void ClassGenerator()
        {
            System.Random Rand = new System.Random(1231415);
            RandomTestClass Item = Rand.NextClass<RandomTestClass>();
            Assert.Equal(202970450, Item.A);
            Assert.Equal("Lorem ipsum dolor sit amet. ", Item.B);
            Assert.Equal(System.Math.Round(0.9043f, 4), System.Math.Round(Item.C, 4));
            Assert.InRange(Item.D, 1, 100);
        }

        [Fact]
        public void Shuffle()
        {
            System.Random Rand = new System.Random(1231415);
            Assert.Equal(new int[] { 3, 1, 4, 5, 2 }, Rand.Shuffle(new int[] { 1, 2, 3, 4, 5 }));
        }
    }

    public class RandomTestClass
    {
        [IntGenerator]
        public int A { get; set; }

        [LoremIpsumGenerator]
        public string B { get; set; }

        [FloatGenerator]
        public float C { get; set; }

        [IntGenerator(1, 100)]
        public int D { get; set; }
    }
}
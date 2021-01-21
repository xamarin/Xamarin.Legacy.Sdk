// This is a C# class that won't compile if the Java bindings aren't generated properly
using System;

class Foo
{
    public Type Type => typeof (GoogleGson.Gson);
}

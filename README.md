# DistributedCache.Extensions

[![NuGet](https://img.shields.io/nuget/v/DistributedCache.Extensions.svg)](https://www.nuget.org/packages/DistributedCache.Extensions/)

A .NET library for easy serialization and deserialization of objects in distributed caches.

## Table of Contents

- [Installation](#installation)
- [Usage](#usage)
  - [Serialization](#serialization)
  - [Deserialization](#deserialization)

## Installation

You can install the DistributedCache.Extensions library via [NuGet](https://www.nuget.org/packages/DistributedCache.Extensions/):

```bash
dotnet add package DistributedCache.Extensions

```


## Usage
### Serialization
To serialize an object and store it in a distributed cache, use the Set method:

```csharp
var user = new User("John", "Snow");
cache.Set("user:john_snow", user);
```

### Deserialization
To retrieve and deserialize an object from a distributed cache, use the Get method:

```csharp
var cachedUser = cache.Get<User>("user:john_snow");
if (cachedUser != null)
{
    // Use the deserialized object.
}
```
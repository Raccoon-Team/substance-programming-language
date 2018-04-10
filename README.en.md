# Substance PL
# About language and compiler
Substance PL is an experimental, object-oriented programming language with support for metalanguage for creating new constructions.

# Key Features:
* Support for the metalanguage for describing your own constructions
* Full-Plugin architecture with the ability to completely replace all compiler modules
* Ability to compile on different platforms without changing the code due to 2-step compilation (source -> IL -> binary)

# About metalanguage and creation of own constructions
Constructions can be described in two ways:
1. IL code
```C#
il
{
  //IL Code here
}
```

2. Based on existing structures
```C#
//Описание конструкции while-else из Python
Interface:

while (|boolExpression|)
{
	|whileBody|
}
else 
{
	|elseBody|
}

Implementation:

var breaked = false;
while (|boolExpression|)
{
	breaked = true;
	|whileBody|
	breaked = false;
}
if (!breaked) 
{
	|elseBody|
}
```

Since the compiler has a Full-Plugin architecture, it will not initially have any constructs. Therefore, the first constructions will be possible to describe only using IL code.

# About compilation
Code translation occurs in 2 stages:
1. Translation of source code into intermediate
1. Translation from intermediate to machine

## License

This project is licensed under the LGPL-3.0 License - see the [LICENSE.md](LICENSE.md) file for details

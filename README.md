# ClearBank Developer Test

## Getting Started
The solution depends on the latest LTS version of the .NET 6 SDK (6.0.402) to be installed as defined in the global.json file.
Ensure you have the latest SDK installed by running 
```bash
dotnet --list-sdks
```

## Disclaimer
As SOLID is defined as a required design principle, having one class with all the logic made no sense. 
Given that the S stands for Single responsibility principle, Robert Martin states that "Every software module should have only one reason to change."
Putting all the logic in a single class would violate this principle as the issue is that your class won't be conceptually cohesive, giving it several reasons to change.
As such, reducing the number of times we modify a class is essential. 

As a result, classes should actually have been split so that they smaller, cleaner, and thus easier to maintain.
To simplify the coding process, I would typically choose to implement a 3-layer architecture. 

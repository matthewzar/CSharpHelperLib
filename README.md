# CSharpHelperLib

CSharpHelperLib started out as a library (or set of libraries) to make common tasks faster and easier. It now has functions that affect a wide range of .Net areas, including (but not limited to):

  - WPF components
  - System.IO extensions
  - Reflection for private field manipulation

## Who is this for?

No one in particular, actually. The primary goal of this project was to create a well formatted practice area (for personal growth/use) that could double as a base for re-usable code.

### Can I contribute?

Of course. Please just be sure to follow a TDD pattern:

* All functions should be thourougly tested before committing.
* Ideally all functions should also have unit test or integration tests.
* Try to keep any static functions [pure](https://en.wikipedia.org/wiki/Pure_function/) - side effects are not good practice*.

All contributions should be made on branches, and all merges should be made through merge requests (rather than straight merges) so that code reviews can be done.

*We know that in some cases impure statics are impossible to avoid - Console.WriteLine() is a static with side-effects after all - but you should still try.

## How can I use this library?

CSharpHelperLib is not listed on NuGet, so to make use of it you need to:
* Pull the repo.
* Perform a local build.
* Copy the generated CollectionOfHelpers.dll file into your project.
* Add a reference in your project.
* Enjoy.



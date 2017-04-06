# Aaron's Visual Studio Templates
### License

All MIT unless noted.

### Copy-Content Template.csproj

This project will simply copy all files in the project with Build Action = Content to the output directory when building.

Useful for simple static HTML/CSS/JS stuff when you don't need anything beyond the files themselves.

Based on the [excellent article by Eilon Lipton](http://weblogs.asp.net/leftslipper/creating-visual-studio-projects-that-only-contain-static-files).

### Zip-Content Template.csproj

This project will simply add all files in the project with Build Action = Content to a zip in the output directory when building.

Useful for creating simple zip packages, or for messing with the guts of Office documents.

Uses [msbuildtasks](https://github.com/loresoft/msbuildtasks) via Nuget.

### Gist.tt

This T4 file will download a C# file from GitHub's gist service and place it in your project.

### CopyFile.tt

This T4 file will copy a C# file from a relative path and place it in your project.

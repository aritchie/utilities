@echo off
del *.nupkg
nuget pack Acr.Core.nuspec
nuget pack Acr.Rx.nuspec
nuget pack Acr.Rx.Android.nuspec
nuget pack Acr.Rx.iOS.nuspec
pause
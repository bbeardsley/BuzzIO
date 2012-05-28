@echo off
nuget pack BuzzIO.csproj -Symbols -Build -OutputDirectory bin -Prop Configuration=Release -Prop Platform=AnyCPU
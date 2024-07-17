FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build

COPY ./Celestin.API /App

WORKDIR /App

RUN dotnet restore

RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/aspnet:3.1

WORKDIR /App

COPY --from=build /App/out .

CMD dotnet /App/Celestin.API.dll
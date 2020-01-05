FROM mcr.microsoft.com/dotnet/core/runtime:3.1

WORKDIR /app

COPY bin/Release/PublishOutput .

EXPOSE 11111
EXPOSE 30000

CMD ["dotnet", "Silo.dll"]
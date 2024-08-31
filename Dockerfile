# Usar a imagem base do Ubuntu
FROM ubuntu:20.04 AS build

# Definir variáveis de ambiente para evitar prompts interativos durante a instalação
ENV DEBIAN_FRONTEND=noninteractive

# Instalar dependências necessárias
RUN apt-get update \
    && apt-get install -y \
       wget \
       apt-transport-https \
       software-properties-common \
       gnupg2

# Adicionar o repositório do Microsoft para MSBuild e .NET
RUN wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb \
    && dpkg -i packages-microsoft-prod.deb \
    && apt-get update

# Instalar o .NET SDK e MSBuild
RUN apt-get install -y dotnet-sdk-6.0 msbuild

# Criar diretório de trabalho
WORKDIR /app

# Copiar arquivos do projeto para o diretório de trabalho
COPY . .

# Restaurar dependências
RUN dotnet restore

# Construir o projeto
RUN dotnet build --no-restore -c Release

# Publicar a aplicação
RUN dotnet publish --no-build -c Release -o /out

# Imagem final para o runtime
FROM ubuntu:20.04 AS runtime

# Instalar dependências necessárias para rodar .NET
RUN apt-get update \
    && apt-get install -y \
       libicu-dev \
       libkrb5-3 \
       zlib1g \
       libssl1.1 \
       liblttng-ust0 \
       libunwind8 \
       libuuid1 \
       libcurl4 \
       libgcc1 \
       libgssapi-krb5-2 \
       libstdc++6 \
       libc6 \
       libpcre3 \
       libssl1.1 \
       krb5-locales \
       libcom-err2 \
       libk5crypto3 \
       libkeyutils1 \
       libkrb5-3 \
       libkrb5support0 \
       libtirpc-dev \
       libunwind8 \
       zlib1g

WORKDIR /app

# Copiar o output da build para o runtime
COPY --from=build /out .

# Definir o entrypoint
ENTRYPOINT ["dotnet", "APIRafael.dll"]

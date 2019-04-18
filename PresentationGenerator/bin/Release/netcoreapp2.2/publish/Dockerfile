FROM microsoft/dotnet:2.2-aspnetcore-runtime
WORKDIR /app
COPY . .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet PresentationGenerator.dll

# soft link
RUN ln -s /lib/x86_64-linux-gnu/libdl-2.24.so /lib/x86_64-linux-gnu/libdl.so

RUN apt-get update \
    && apt-get install -y --allow-unauthenticated \
   		libc6-dev \
		libgdiplus \
		libx11-dev \
     && rm -rf /var/lib/apt/lists/*
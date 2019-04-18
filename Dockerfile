FROM docker.io/microsoft/dotnet:2.2-aspnetcore-runtime
COPY . /app
WORKDIR /app    

# soft link
RUN ln -s /lib/x86_64-linux-gnu/libdl-2.24.so /lib/x86_64-linux-gnu/libdl.so

# install System.Drawing native dependencies
RUN apt-get update \
    && apt-get install -y --allow-unauthenticated \
   		libgdiplus \
		libc6-dev \
		libx11-dev \
     && rm -rf /var/lib/apt/lists/*
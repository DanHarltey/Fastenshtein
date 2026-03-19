## Notes on how benchmark on Amazon Linux 2023 (AL2023)

The default user has the username ec2-user. Can use like ec2-user@63.35.177.218. Use a private key for ssh authentication.

## How to install .Net

1. Update the package manager
``` 
sudo dnf update -y
```

2. Install .Net required dependencies
```
sudo dnf install -y \
  icu libicu \
  openssl-libs \
  zlib \
  krb5-libs \
  libunwind \
  lttng-ust \
  ca-certificates
``` 

3. Install .Net
```
cd /tmp
wget https://builds.dotnet.microsoft.com/dotnet/scripts/v1/dotnet-install.sh
chmod +x dotnet-install.sh

# Install latest 10.0 SDK into ~/.dotnet (you can also pin an exact version)
./dotnet-install.sh --channel 10.0

echo 'export DOTNET_ROOT=$HOME/.dotnet' >> ~/.bashrc
echo 'export PATH=$PATH:$DOTNET_ROOT:$DOTNET_ROOT/tools' >> ~/.bashrc
source ~/.bashrc
```

4. Test install
```
dotnet --info
```

## Run benchmarks
```
cd Fastenshtein
dotnet build --configuration Release

cd benchmarks/Fastenshtein.Benchmarking/bin/Release/net10.0/

dotnet Fastenshtein.Benchmarking.dll c
```

## Last benchmark
For the last benchmark I used 
- c7a.xlarge - AMD EPYC 9R14 3.70GHz, 1 CPU, 4 logical and 4 physical cores al2023-ami-2023.10.20260120.4-kernel-6.1-x86_64
- c8g.large - al2023-ami-2023.10.20260120.4-kernel-6.12-arm64
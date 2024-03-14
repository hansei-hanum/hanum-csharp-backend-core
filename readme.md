# Hanum Csharp Backend Core
한움 마이크로 서비스 코어 패키지

[Hanum Common Authentication](https://github.com/hansei-hanum/hanum-csharp-backend-core/blob/main/Authentication/HanumAuthenticationHandler.cs), [Standard DTO](https://github.com/hansei-hanum/hanum-csharp-backend-core/tree/main/Models/DTO), [User Service](https://github.com/hansei-hanum/hanum-csharp-backend-core/blob/main/Services/IHanumUserService.cs), [Notification Service](https://github.com/hansei-hanum/hanum-csharp-backend-core/blob/main/Services/IHanumNotificationService.cs) 등이 포함되어있습니다.

## Nuget 패키지 소스 추가 방법
이 레포지토리를 nuget 패키지로 사용하려면 hansei-hanum 조직의 nuget source 등록이 필요합니다.

### 소스 등록 방법
[NuGet 레지스트리 작업
](https://github.com/hansei-hanum/hanum-csharp-backend-core)의 내용처럼 [Personal access tokens (classic)](https://github.com/settings/tokens) 토큰 발급 후 아래 명령어를 실행시켜 nuget 소스를 추가합니다.
```sh
dotnet nuget add source --username USERNAME --password GITHUB_TOKEN --store-password-in-clear-text --name hanum "https://nuget.pkg.github.com/hansei-hanum/index.json"
```

#### CLI에서 패키지 설치
아래 명령어를 통해 패키지를 설치합니다.
```sh
dotnet add package Hanum.Core --version x.x.x
```

#### Visual Studio 패키지 관리자로 패키지 설치

1. 아래 사진처럼 nuget 소스를 아까 추가한 hanum으로 변경합니다.
   
   ![image](https://github.com/hansei-hanum/hanum-csharp-backend-core/assets/34199905/f8b310be-20cc-4113-8524-4405f92aae40)
   
3. `Hanum.Core`를 검색하여 패키지를 설치합니다.
   
   ![image](https://github.com/hansei-hanum/hanum-csharp-backend-core/assets/34199905/63dcf549-4f4b-42e0-9276-29391efc9dec)

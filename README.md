# Npgg.ApiRouter
API Router for AWS Lambda via APIGatewayProxyRequest


# Web API 와의 차별점

Http API 동작만을 위해 개발되었으며,
AWS Lambda Coldstart에서 발생하는 라이브러리를 최소화 하여 초기 동작시간을 크게 낮춰줌.

( Web API 에 비해 약 30~50%의 빠른 콜드스타트 처리시간을 갖음 )


#주요기능
- API Url Pattern 에 따른 API 함수 연결
- 단위테스트에서 http call 을 따르지 않고 사용 가능
- 필요에 따라 .NET CORE Web API에서 호출가능
- Generic 한 MultiValueHeader/QueryString 활용
- 기본적인 사용방법이 .NET CORE Web API 와 크게 다르지 않아 


# 예제

Http Get을 url patter 으로 받을때

```csharp
  [HttpGet("api/get/{id}")]
  public int Get(int id)
  {
    //do something
  }
```

Http Post의 body 값을 인자로 받고싶을 때

```csharp
  [HttpPost("api/set")]
  public int Get([FromBody] SomeData bodyObject)
  {
    //do something
  }
```

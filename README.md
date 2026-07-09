# specflow_web

C# Web 自动化 Demo：使用 **PuppeteerSharp** + **Reqnroll** 实现 [SauceDemo](https://www.saucedemo.com) 登录成功测试。

## 技术栈

| 组件 | 用途 |
|------|------|
| .NET 10 / NUnit | 测试运行框架 |
| Reqnroll | BDD（Gherkin Feature / Step Definitions） |
| PuppeteerSharp | Chrome 浏览器自动化 |
| Reqnroll HTML Formatter | 测试报告（零额外依赖） |

## 项目结构

```
SpecFlowWeb.Tests/
├── Drivers/BrowserDriver.cs
├── Pages/
├── Features/Login.feature
├── StepDefinitions/
├── Hooks/Hooks.cs
└── reqnroll.json

TestResults/report.html    # 测试结束后自动生成
```

## 前置条件

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- 有头模式需要本机安装 **Google Chrome**
- 无头模式首次运行会自动下载 Chromium 到 `~/.cache/specflow_web/chromium`

## 运行测试

```bash
dotnet test
```

有头模式（使用本机 Google Chrome，测完立即关闭）：

```bash
HEADED=true dotnet test
```

需要观察结果页时，可指定结束后停留时间（毫秒）：

```bash
HEADED=true HEADED_PAUSE_MS=5000 dotnet test
```

测试结束后用浏览器打开 `TestResults/report.html` 查看报告。

## 测试账号

来自 [SauceDemo](https://www.saucedemo.com) 公开测试站：

- 用户名：`standard_user`
- 密码：`secret_sauce`

## 场景说明

`Login.feature` 覆盖：打开登录页 → 输入有效凭据 → 断言进入商品库页面且标题为 `Products`。

## CI

Push / PR 时 GitHub Actions 会执行无头测试，并在 Artifacts 中上传 `test-report-html`。

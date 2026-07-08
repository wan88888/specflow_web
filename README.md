# specflow_web

C# Web 自动化 Demo：使用 **PuppeteerSharp** + **Reqnroll**（SpecFlow 的开源继任者）实现 [SauceDemo](https://www.saucedemo.com) 登录成功测试。

## 技术栈

| 组件 | 用途 |
|------|------|
| .NET 8 / NUnit | 测试运行框架 |
| Reqnroll | BDD（Gherkin Feature / Step Definitions，兼容 SpecFlow） |
| PuppeteerSharp | 无头 Chrome 浏览器自动化 |

## 项目结构

```
SpecFlowWeb.Tests/
├── Drivers/BrowserDriver.cs          # Puppeteer 浏览器生命周期
├── Pages/LoginPage.cs                # 登录页 Page Object
├── Pages/InventoryPage.cs            # 商品页 Page Object
├── Features/Login.feature            # Gherkin 场景
├── StepDefinitions/LoginStepDefinitions.cs
└── Hooks/Hooks.cs                    # 场景前后启动/关闭浏览器
```

## 前置条件

- [.NET 8 SDK](https://dotnet.microsoft.com/download) 或更高版本
- 首次运行时 PuppeteerSharp 会自动下载 Chromium

## 运行测试

```bash
dotnet test
```

有头模式（可见浏览器窗口）：

```bash
HEADED=true dotnet test
```

## 测试账号

来自 [SauceDemo](https://www.saucedemo.com) 公开测试站：

- 用户名：`standard_user`
- 密码：`secret_sauce`

## 场景说明

`Login.feature` 覆盖：打开登录页 → 输入有效凭据 → 断言进入商品库页面且标题为 `Products`。

## CI

Push / PR 到 `main` 或 `master` 时，GitHub Actions 会执行无头测试：

- Checks 页可查看 **SpecFlow Web Tests** 报告
- Artifacts 中可下载 `test-results`（TRX）

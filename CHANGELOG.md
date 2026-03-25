# Changelog

## 0.1.7 (2026-03-24)

- Add unit tests
- Add test step to CI workflow

## 0.1.6 (2026-03-23)

- Sync .csproj description with README

## 0.1.5 (2026-03-22)

- Add dates to changelog entries

## 0.1.4 (2026-03-16)

- Add Development section to README
- Add GenerateDocumentationFile and RepositoryType to .csproj

## 0.1.3 (2026-03-16)

## 0.1.1 (2026-03-13)

- Include README in NuGet package

## 0.1.0 (2026-03-13)

- Initial release
- In-memory background task queue backed by `System.Threading.Channels`
- Configurable concurrency via `MaxConcurrency` option
- Bounded or unbounded queue via `MaxQueueSize` option
- Error handling callback via `OnError` option
- One-line DI registration with `AddBackgroundTaskQueue()`

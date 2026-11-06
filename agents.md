# KADRO-APP: REPOSITORY GUIDE

windows desktop task and board manager. this guide covers the repository; nested agent instructions apply within their own folders.

## PROJECT OVERVIEW

| property | verified value |
| --- | --- |
| repository | `rezzt-dev/kadro-app` |
| active source | `craftingTask/` |
| development base for this change | `main` |
| remote default branch | `main` |
| version source | `craftingTask/craftingTask.csproj` |
| continuous integration | no tracked github actions workflows |

## TECHNOLOGY STACK

- .net project files declare target frameworks and package references; preserve platform-specific requirements

## PROJECT STRUCTURE

- `DatabaseInitializationTest.cs`
- `DatabaseTester.cs`
- `LICENSE`
- `README.md`
- `changelog.md`
- `craftingTask/`

management documentation lives in `.docs/manager.md`, `.docs/-github-guides/` and `.docs/repo-audit-and-fix.md`. keep this file and `claude.md` consistent.

## BUILD, RUN AND TEST COMMANDS

for application changes, run these checks from `craftingTask/`:

```bash
dotnet build craftingTask.csproj
```

## MODULE RESPONSIBILITIES

| path | responsibility |
| --- | --- |
| `craftingTask/craftingTask.csproj` | actual wpf application project and package references |
| `craftingTask/App.xaml` | application resources |
| `craftingTask/model/objects/` | tasks, boards, statuses, reminders and search models |
| `craftingTask/model/services/` | application service contracts and implementations |
| `craftingTask/persistence/managers/` | persisted entities and backup operations |
| `craftingTask/view/frame_pages/` | board, calendar, reminder and search screens |
| `craftingTask/view/windows/` | main window and dialogs |
| `craftingTask/resources/` | bundled database schema and presentation resources |

the project targets `net8.0-windows7.0` with wpf. use a compatible windows desktop build environment for application verification. the `craftingTask/craftingTask_0ppobpvm_wpftmp.csproj` project and `obj/` outputs are generated material, not an alternative application definition. validate persistence changes against a disposable sqlite copy; preserve backup and restore behaviour.

## CODE STYLE AND ARCHITECTURAL DECISIONS

preserve local sqlite data and wpf bindings; generated obj/bin/.vs files are not source; do not rebuild or publish shipped installers without authorization. follow the existing formatter and naming of the changed module. verify manifest versions when dependencies change.

## TESTING STRATEGY

documentation-only changes require `git diff --check`, existing-path checks and a review of the staged file list. run module checks for application changes, with disposable data. report unavailable tooling and failed checks; never claim a check passed without running it.

## SECURITY AND DEPLOYMENT

keep credentials, local configuration, signing material and user data out of commits. do not print secret values while auditing. inspect configuration names and code references instead.

existing legal and community files: `LICENSE`. when changing dependencies, permissions, storage or network services, review those files against the implementation. record missing policies as audit work; do not invent licence terms or ownership decisions.

deployment, signing, store publication, migrations against hosted databases and repository settings require their own explicit request. this documentation task prepares management instructions and pull requests.

## COMMON TASKS

1. read the relevant module and its nearest agent instructions
2. inspect the working tree and choose the smallest change that solves the request
3. update the affected documentation and changelog with evidence from the diff
4. run checks appropriate to the changed module and report their actual results
5. for git operations, follow the manager and the digest below

## COMMIT, PR, MERGE & RELEASE GUIDELINES

read `.docs/manager.md` first, then the selected guide in `.docs/-github-guides/`. these files are versioned; this digest also works without opening them.

| request | guide |
| --- | --- |
| commit title, body and changelog | `commit-guide.md`, `changelog-guide.md` |
| pull request or merge | `pr-merge-guide.md`, `branching-guide.md` |
| version and release | `release-guide.md`, `changelog-guide.md` |
| permissions and sensitive data | `agent-rules.md` |

- inspect `git status`, both diffs and recent history before describing changes
- use `<prefix> | <imperative summary>` with `update`, `fix`, `hotfix`, `changes` or `changelog`; keep titles within 72 characters
- write commit bodies as 2 to 6 markdown bullets, one logical change per line, with literal paths in backticks
- never add authorship footers, automated attribution, signatures or generation notices; preserve the configured git identity
- update `changelog.md` under `## SIN PUBLICAR / UNRELEASED` in matching `### CASTELLANO` and `### ENGLISH` sections
- use the existing changelog category vocabulary and preserve language parity; describe only changes in the actual diff
- summarize pull requests in 1 to 3 bullets per category and language, at most 110 characters per bullet
- documentation and repository-management work targets `main`; the remote default is `main`; do not silently promote development work to the default branch
- the authorization for a requested action remains valid throughout that task; creating a pull request includes pushing its branch, but does not authorize merging or publishing
- use merge commits when a merge is requested; never force-push, rewrite published history or remove branches without an explicit instruction
- releases use the approved version and an immutable tag on the verified release commit, with english notes from the closed changelog; do not create a release for documentation work
- deliver requested titles, bodies and changelog fragments in separate code blocks; report executed actions with their commit hash or pull request link

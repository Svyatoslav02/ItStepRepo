# Git Workflow

This document describes the Git workflow for the MoodboardAI project.

The goal of this workflow is to keep the `main` branch stable and organize team development through issues, feature branches, pull requests, and code review.

## Main rules

* Direct push to `main` is not allowed.
* All changes must go through a Pull Request.
* Each Pull Request must be linked to a GitHub Issue.
* Each Pull Request must target the `main` branch.
* Pull Requests require 2 approvals before merge.
* Code Owners must review changes.
* All review comments must be resolved before merge.
* Force push to protected branches is blocked.
* Branch deletion for protected branches is blocked.
* Only the project lead merges Pull Requests into `main`.

## Branches

The repository uses `main` as the protected production branch.

The `main` branch contains only stable and reviewed code.

Students must create separate branches for each task.

## Branch naming rules

Use clear branch names based on the task type and issue title.

Recommended format:

```bash
type/short-task-name
```

Allowed branch prefixes:

```txt
feature/
bugfix/
docs/
refactor/
test/
```

Examples:

```bash
feature/generate-moodboard-endpoint
feature/frontend-prompt-form
docs/git-workflow
docs/api-contract
bugfix/prompt-validation
refactor/moodboard-service
test/generate-endpoint-tests
```

Do not use vague branch names:

```bash
test
new-code
fix
my-branch
changes
task
```

## Workflow steps

### 1. Get the latest main branch

Before starting a new task, update your local `main` branch.

```bash
git checkout main
git pull origin main
```

### 2. Create a new branch

Create a branch for your task.

```bash
git checkout -b feature/task-name
```

Example:

```bash
git checkout -b feature/generate-moodboard-endpoint
```

### 3. Work only inside your task scope

Each branch should solve one task.

Do not mix unrelated changes in one branch.

Good example:

```txt
Add POST /api/generate endpoint
```

Bad example:

```txt
Add backend endpoint, change frontend layout, update README, rename folders
```

### 4. Check your changes locally

Before committing, check that the project builds.

For backend:

```bash
cd backend
dotnet restore
dotnet build
dotnet test
```

For frontend:

```bash
cd frontend
npm install
npm run build
```

### 5. Commit your changes

Use clear commit messages.

Recommended format:

```bash
git commit -m "type: short description"
```

Examples:

```bash
git commit -m "feature: add generate moodboard endpoint"
git commit -m "docs: add git workflow documentation"
git commit -m "bugfix: fix empty prompt validation"
git commit -m "test: add generate endpoint tests"
git commit -m "refactor: move moodboard logic to service"
```

Avoid unclear commit messages:

```bash
git commit -m "update"
git commit -m "fix"
git commit -m "changes"
git commit -m "done"
```

### 6. Push your branch

Push your branch to GitHub.

```bash
git push origin feature/task-name
```

Example:

```bash
git push origin feature/generate-moodboard-endpoint
```

### 7. Create a Pull Request

Open GitHub and create a Pull Request from your branch into `main`.

Pull Request target:

```txt
base: main
compare: your-feature-branch
```

Every Pull Request must include:

* Clear title
* Short description
* Related issue number
* Completed checklist
* Screenshots for frontend changes
* Notes about tests or manual checks

## Pull Request title rules

Use clear Pull Request titles.

Examples:

```txt
Add generate moodboard endpoint
Create frontend prompt form
Add Git workflow documentation
Update API contract for generate endpoint
```

Avoid unclear titles:

```txt
Update
Fix
Task done
My changes
```

## Linking Pull Requests to Issues

Each Pull Request must be linked to an issue.

Use one of these phrases in the Pull Request description:

```txt
Closes #1
Fixes #1
Resolves #1
```

Example:

```txt
Closes #7
```

After the Pull Request is merged, GitHub will close the linked issue automatically.

## Pull Request description template

Use this structure:

```md
## Description

Short description of what was done.

## Related issue

Closes #

## Changes

- Added ...
- Updated ...
- Fixed ...

## How it was tested

- [ ] Build passed locally
- [ ] Tests passed locally
- [ ] Manual check completed

## Screenshots

Add screenshots for UI changes.
```

## Review process

After creating a Pull Request:

1. Wait for reviewers.
2. Do not merge your own Pull Request.
3. Answer review comments.
4. Push fixes to the same branch.
5. Wait until all comments are resolved.
6. Wait for 2 approvals.
7. The project lead merges the Pull Request.

## Approval rules

Pull Requests require:

```txt
2 approvals
```

Code Owners must review changes.

Current Code Owners are defined in:

```txt
.github/CODEOWNERS
```

Example:

```txt
* @Svyatoslav02 @lvalexz
```

This means all Pull Requests require review from the configured owners.

## Main branch protection rules

The `main` branch is protected.

Current protection rules:

* Pull Request is required before merge
* 2 approvals are required
* Code Owner review is required
* Stale approvals are dismissed after new commits
* Review conversations must be resolved before merge
* Force pushes are blocked
* Branch deletion is blocked

These rules protect the repository from accidental or unreviewed changes.

## Updating a Pull Request after review comments

If reviewers request changes, update the same branch.

```bash
git checkout feature/task-name
```

Make changes.

```bash
git add .
git commit -m "fix: address review comments"
git push origin feature/task-name
```

Do not create a new Pull Request for review fixes.

## Keeping your branch up to date

If `main` was updated, sync your branch.

```bash
git checkout main
git pull origin main
git checkout feature/task-name
git merge main
```

Resolve conflicts if needed.

Then push:

```bash
git push origin feature/task-name
```

## Conflict resolution rules

If a merge conflict appears:

1. Read the conflict carefully.
2. Keep only the correct final version.
3. Build the project after resolving conflicts.
4. Run tests if the project has tests.
5. Commit the conflict fix.

Example:

```bash
git add .
git commit -m "fix: resolve merge conflicts"
git push origin feature/task-name
```

Do not delete another student's work without review.

## Issue status workflow

GitHub Issues should move through these statuses:

```txt
Backlog
Ready
In Progress
Code Review
Testing
Done
```

Status meaning:

```txt
Backlog: task exists, but work has not started
Ready: task is ready for development
In Progress: student is working on the task
Code Review: Pull Request is created
Testing: changes are being checked
Done: Pull Request is merged and issue is closed
```

## Student responsibilities

Each student is responsible for:

* Taking only assigned issues
* Creating a separate branch for each issue
* Writing clear commit messages
* Creating Pull Requests into `main`
* Linking Pull Requests to issues
* Responding to review comments
* Keeping the branch focused on one task
* Not committing real secrets or API keys

## Project lead responsibilities

The project lead is responsible for:

* Managing GitHub Issues
* Managing GitHub Project board
* Reviewing Pull Requests
* Merging approved Pull Requests
* Keeping the `main` branch stable
* Updating branch protection rules
* Defining project workflow rules

## Secrets and environment files

Do not commit real secrets.

Do not commit:

```txt
.env
API keys
database passwords
JWT secrets
private tokens
```

Only example files are allowed:

```txt
.env.example
```

If a real secret was committed by mistake:

1. Revoke the secret.
2. Generate a new key.
3. Remove the secret from the repository.
4. Notify the project lead.

## Recommended local check before Pull Request

Before opening a Pull Request, run the required checks.

Backend:

```bash
cd backend
dotnet restore
dotnet build
dotnet test
```

Frontend:

```bash
cd frontend
npm install
npm run build
```

Documentation:

```txt
Check spelling
Check markdown formatting
Check file names
Check links if links were added
```

## Final checklist before Pull Request

Before opening a Pull Request, check:

* [ ] Branch name is clear
* [ ] Pull Request targets `main`
* [ ] Related issue is linked
* [ ] Changes match the task scope
* [ ] No unrelated files are changed
* [ ] No secrets are committed
* [ ] Project builds locally
* [ ] Tests pass if tests exist
* [ ] Documentation is updated if needed

## Summary

This workflow helps the team work safely and consistently.

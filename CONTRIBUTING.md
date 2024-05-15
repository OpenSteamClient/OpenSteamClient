# Contributing to OpenSteamClient
We love your input! We want to make contributing to this project as easy and transparent as possible, whether it's:

- Reporting a bug
- Discussing the current state of the code
- Submitting a fix
- Proposing new features
- Becoming a maintainer


## All Code Changes Happen Through Pull Requests
Pull requests are the best way to propose changes to the codebase. We actively welcome your pull requests:

1. Check that nobody is working on the issue you want to tackle
2. Fork the repo and create your branch from `master`.
3. Send a draft PR
4. Add a descriptive description and optionally some checkboxes with the objectives so people can see at a glance how you're doing, and what they can do to help
5. Add features/Fix bugs/Write Code
6. Switch the PR from Draft to Open!
7. Your PR will be reviewed as the maintainers have time
  - Seen no acknowledgement in a few days? Try tagging a maintainer!

## `help-wanted` PRs
If someone has a draft PR that is tagged with `help-wanted`, you can also contribute to their fork directly through that fork's Pull Requests. These are called "Sub-PRs".
PRs are considered to be reviewable when you change it's status from Draft to Open (you can also ask for comments and code reviews anytime by leaving a comment on the PR)

## Any contributions you make will be under the MIT Software License
In short, when you submit code changes, your submissions are understood to be under the [MIT License](http://choosealicense.com/licenses/mit/) that covers the project.
By contributing, you agree that your contributions will be licensed under its MIT License.

## Report bugs using Github's [issues](https://github.com/OpenSteamClient/OpenSteamClient/issues)
We use GitHub issues to track public bugs. Report a bug by [opening a new issue](https://github.com/OpenSteamClient/OpenSteamClient/issues/new); it's that easy!

## Write bug reports with detail, background, and sample code
**Great Bug Reports** tend to have:

- A quick summary and/or background
- Steps to reproduce
  - Be specific!
  - If you're using our APIs or something that involves your own code, please provide a minimal reproducible example.
- What you expected would happen
- What actually happens
- Screenshots (If applicable)
- Notes (possibly including why you think this might be happening, or stuff you tried that didn't work)


## Coding Style
Try to follow the code that's written in that file/directory/codebase and write similarly. 

### Properties/Methods/Fields for data access
Use methods for computationally intensive tasks (that may have side effects), for example deserializing binary.
Use properties for simple, computationally light tasks like redirecting to an underlying object. 
If properties have side effects, 
Do not expose fields, except for `internal` use.

### Long-running tasks
For long running tasks (for example, updating persona states in the background), prefer using Task.Run or Thread.Start to make an always-running task or thread.
For long running, oneshot tasks (for example logging in or updating appinfo), use async Task(s), and report progress via IExtendedProgress

### Tests
We don't currently have tests, as the project is still rapidly taking shape.

## Footnote by Rosentti
This is my first open-source project that I've led.
I may make some mistakes or bad decisions every now and then and if that's the case feel free to suggest and correct :)

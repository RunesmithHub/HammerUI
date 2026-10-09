# Contributing to HammerUI

Thanks for helping. This page has the conventions for code, documentation and commits.

## Before you start

- For a bug, [open an issue](https://github.com/RunesmithHub/HammerUI/issues/new/choose) with the steps to reproduce it. For a security
  problem, follow [SECURITY.md](SECURITY.md) instead, so it is reported privately.
- For a larger change, such as a new control, open an issue first, so the approach is agreed on before you write the code.

## On NixOS

Work in the development shell from `flake.nix` (`nix develop`, or direnv with `.envrc`) and start your editor from it
(`nix develop -c <editor>`); otherwise the gallery cannot load its font and X11 libraries.

## Making a change

1. Fork [RunesmithHub/HammerUI](https://github.com/RunesmithHub/HammerUI) and create a branch from `main`.
2. Build and test:

   ```bash
   dotnet build HammerUI.slnx -warnaserror
   dotnet test --solution HammerUI.slnx -- --ignore-exit-code 8
   ```

3. Keep the formatting: `dotnet format whitespace HammerUI.slnx` and `dotnet format style HammerUI.slnx` fix it.
4. When a control, a theme resource or an API changes, update the documentation in `website/` in the same commit; `website/STYLE.md`
   covers how it is written. Show new controls in the gallery, in light and dark.
5. Write each commit message as a [Conventional Commit](https://www.conventionalcommits.org/en/v1.0.0/), such as
   `fix(docking): keep the tab order when a window closes`. The types are `feat`, `fix`, `perf`, `refactor`, `test`, `docs`, `ci`,
   `chore` and `revert`; the scope names the part of the library, such as `docking`, `theme`, `icons` or `controls`.
6. Open a pull request against `main` and fill in the template.

## Code

- The library builds without warnings; CI builds with `-warnaserror`.
- Public types and members have XML documentation that says what they do for the caller.
- Comments explain only what the code cannot say, in one line where possible.
- Controls take their colors, sizes and corner radii from theme resources, so every control follows the light and dark palettes and the
  accent color.

## License

HammerUI is licensed under the [Apache License 2.0](LICENSE). Contributions you submit are licensed under the same terms, as section 5 of
the license describes.

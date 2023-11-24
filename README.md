# PortListApp

This is a personal tool that is not expected to be useful (or potentially even
functional) for anyone else. The code's not great, but it does what I need it
to do. You may use it if you want, but I won't provide support. PRs welcome
(especially if they include a better icon).

## Instructions

Build with Visual Studio 2022. Currently only configured for Windows 11,
but it would probably work on older versions.

While running, PortListApp places an icon in the notification area ("system
tray"). Right clicking on the icon brings up a menu with a list of ports.
Clicking on a port will open a serial terminal (currently KiTTY with a
hardcoded path at 115200 baud 8-N-1, no flow control; see code for
details).

## License

This program is free software: you can redistribute it and/or modify it under
the terms of the GNU Affero General Public License as published by the Free
Software Foundation, either version 3 of the License, or (at your option) any
later version.

This program is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A
PARTICULAR PURPOSE. See the GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License along
with this program. If not, see <https://www.gnu.org/licenses/>.

See `LICENSE.txt` for details.

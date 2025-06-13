#!/usr/bin/env python3

import atexit
import base64
import ctypes
import gzip
import json
import os
import random
import socket
import struct
import subprocess as sp
import threading
import time

from ctypes import wintypes
from typing import Any, Optional


"""LoopstreamUnbox.py: send tags from unbox to Loopstream"""
__version__   = "1.0"
__author__    = "ed <loopstream@ocv.me>"
__credits__   = ["stackoverflow.com"]
__license__   = "MIT"
__copyright__ = 2025


def disable_quickedit() -> None:
    def ecb(ok: bool, fun: Any, args: "list[Any]") -> "list[Any]":
        if not ok:
            err: int = ctypes.get_last_error()  # type: ignore
            if err:
                raise ctypes.WinError(err)  # type: ignore
        return args

    k32 = ctypes.WinDLL(str("kernel32"), use_last_error=True)  # type: ignore

    k32.GetStdHandle.errcheck = ecb  # type: ignore
    k32.GetConsoleMode.errcheck = ecb  # type: ignore
    k32.SetConsoleMode.errcheck = ecb  # type: ignore
    k32.GetConsoleMode.argtypes = (wintypes.HANDLE, wintypes.LPDWORD)
    k32.SetConsoleMode.argtypes = (wintypes.HANDLE, wintypes.DWORD)

    def cmode(out: bool, mode: Optional[int] = None) -> int:
        h = k32.GetStdHandle(-11 if out else -10)
        if mode:
            return k32.SetConsoleMode(h, mode)  # type: ignore

        cmode = wintypes.DWORD()
        k32.GetConsoleMode(h, ctypes.byref(cmode))
        return cmode.value

    # disable quickedit
    mode = orig_in = cmode(False)
    quickedit = 0x40
    extended = 0x80
    mask = quickedit + extended
    if mode & mask != extended:
        atexit.register(cmode, False, orig_in)
        cmode(False, mode & ~mask | extended)

    # enable colors in case the os.system("rem") trick ever stops working
    mode = orig_out = cmode(True)
    if mode & 4 != 4:
        atexit.register(cmode, True, orig_out)
        cmode(True, mode | 4)


def main():
    print("connecting to 127.0.0.1 port 8080 (which should be unbox)")
    sck = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    sck.setsockopt(socket.IPPROTO_TCP, socket.TCP_NODELAY, 1)
    try:
        sck.connect(("127.0.0.1", 8080))
    except:
        print("failed; trying to launch unbox in a new window...")
        try:
            sp.check_call(["cmd", "/c", "start", "unbox"])
            zs = "\n  i have taken the liberty of launching unbox.exe in another window; please select your DJ software over there\n"
            print(zs)
        except:
            zs = "\n  nevermind, failed to connect to unbox, probably because it's not running, and also failed to launch unbox, probably because unbox.exe doesn't exist inside the same folder as LoopstreamUnbox.py ...\n\n  to fix this, download and extract this zipfile: https://github.com/erikrichardlarson/unbox/releases/latest/download/unbox.exe.zip\n"
            print(zs)
            return
        while True:
            try:
                time.sleep(0.5)
                sck.connect(("127.0.0.1", 8080))
                break
            except:
                print(".", end="", flush=True)
    print("connected, now switching protocol to websocket")

    ws_key = base64.b64encode(random.randbytes(16)).decode("ascii")
    zs = f"""
GET /ws HTTP/1.1
Host: 127.0.0.1:8080
Connection: Upgrade
Upgrade: websocket
Sec-WebSocket-Version: 13
Sec-WebSocket-Key: {ws_key}
"""
    zs = (zs.strip().replace("\n", "\r\n") + "\r\n\r\n")
    sck.send(zs.encode("utf-8"))

    buf = b""
    err = "got a bad handshake response from unbox:\n"
    while True:
        zb = sck.recv(1 if buf.endswith(b"\r") or buf.endswith(b"\n") else 3)
        if not zb or len(zb) > 4096:
            raise Exception(err + repr(buf))
        buf += zb
        if buf.endswith(b"\r\n\r\n"):
            print("eeey got a handshake response")
            break

    if not buf.startswith(b"HTTP/1.1 101 "):
        raise Exception(err + repr(buf))

    print("successfully chatting with unbox (nice)")

    abspath = os.path.abspath("lsu-np.txt")
    zs = f"""
<src>{abspath}</src><ptn>(.*)</ptn><tit>unbox</tit><desc /><reader>File</reader><freq>500</freq><grp>1</grp><bnc>1</bnc><urldecode>false</urldecode><encoding>utf-8</encoding><yield>{{1}}</yield>
"""
    zb = gzip.compress(zs.strip().encode("utf-8"))
    zb = b"##gz#" + base64.b64encode(zb) + b"##"
    sp.Popen([b"clip"], stdin=sp.PIPE).communicate(zb)

    zs = """
NOTE: if this is the first time you're running LoopstreamUnbox.py
 from this folder, you will now need to tell Loopstream where to
 read tags from. The information that Loopstream needs has been
 generated and written to your clipboard.
Here's how to do that:
 Open Loopstream, click Settings, Tags, Load
"""
    print(zs)

    err = "oh no! lost connection to unbox"
    buf = b""
    while True:
        # start of new frame, okay we need the length
        buf = b""
        while len(buf) < 4:
            zb = sck.recv(1)
            if not zb:
                raise Exception(err)
            buf += zb

        zi1, zi2 = struct.unpack(b"BB", buf[:2])
        need = 2 if zi2 < 126 else 4 if zi2 == 126 else 10 if zi2 == 127 else 0
        msg_len = zi2
        if zi1 != 0x81 or not need:
            zs = "ah fuck, unbox sent a message which LoopstreamUnbox is too dumb to understand orz\nplease send this to ed on irc or as a github issue or something:\n"
            raise Exception(zs + repr(buf))

        print(f"frame header is {need} bytes")
        while len(buf) < need:
            zb = sck.recv(1)
            if not zb:
                raise Exception(err)
            buf += zb

        if need > 2:
            msg_len = struct.unpack(b">H" if need == 4 else b">Q", buf[2:need])[0]
        buf = buf[need:]

        print(f"message len is {msg_len} bytes")
        while len(buf) < msg_len:
            zb = sck.recv(msg_len - len(buf))
            if not zb:
                raise Exception(err)
            buf += zb

        msg = buf.decode("utf-8", "replace")
        #print("got msg: " + msg)

        zj = json.loads(msg)
        artist = zj.get("artist", "").strip()
        title = zj.get("track", "").strip()
        if artist and title:
            tags = f"{artist} - {title}"
        else:
            tags = title or artist

        print("got tags: " + tags)
        with open("lsu-np.txt", "wb") as f:
            f.write(tags.encode("utf-8", "replace"))


if __name__ == "__main__":
    disable_quickedit()

    # reading a socket on windows blocks ^C
    # pain peko
    threading.Thread(target=main, daemon=True).start()
    while True:
        time.sleep(0.1)

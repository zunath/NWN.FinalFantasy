import os
import glob
import re
import winreg
import argparse
import shutil
import stat
from distutils.dir_util import copy_tree
from distutils.dir_util import remove_tree

def find(name, path):
    for root, dirs, files in os.walk(path):
        if name in files:
            return os.path.join(root, name)

def remove_regex(path, regex = "*"):
    files = glob.glob(path + "\\" + regex)
    for f in files:
        os.remove(f)

def copy_folder_content(src, dst):
    if os.path.exists(dst):
        shutil.rmtree(dst)
    shutil.copytree(src, dst)
 
registery = winreg.ConnectRegistry(None, winreg.HKEY_CURRENT_USER)
registry_key = winreg.OpenKey(registery, r"Software\Microsoft\Windows\CurrentVersion\Explorer\User Shell Folders", 0, winreg.KEY_READ)
document_folder, regtype = winreg.QueryValueEx(registry_key, "Personal")
winreg.CloseKey(registry_key)
winreg.CloseKey(registery)
document_folder = os.path.expandvars(document_folder)
print(document_folder)
nwn_destination_hd0 = None
nwn_destination_hak = None
nwn_destination_mod = None
nwn_destination_tlk = None

for folder in glob.glob(document_folder + "\\Neverwinter Nights*"):
    ini_file_location = find("nwn.ini", folder)
    if ini_file_location is not None:
        with open(ini_file_location, "r") as ini_file:
            lines = ini_file.readlines()

            for line in lines:
                if re.search(r'HD0=', line):
                    nwn_destination_hd0 = line.replace('HD0=','').replace("\n", "")
                if re.search(r'HAK=', line):
                    nwn_destination_hak = line.replace('HAK=','').replace("\n", "")
                if re.search(r'TLK=', line):
                    nwn_destination_tlk = line.replace('TLK=','').replace("\n", "")
                if re.search(r'MODULES=', line):
                    nwn_destination_mod = line.replace('MODULES=','').replace("\n", "")

        ini_file.close()

ap = argparse.ArgumentParser()

hak_path = None
tlk_path = None
dotnet_path = None
module_path = None
docker_path = None

ap.add_argument("-k", "--hak", required=False,
   help="path to project hak folder")
ap.add_argument("-t", "--tlk", required=False,
   help="path to project tlk fodler")
ap.add_argument("-m", "--module", required=False,
   help="path to project module folder")
ap.add_argument("-b", "--binary", required=False,
   help="path to project .net binaries folder")
ap.add_argument("-d", "--docker", required=False,
   help="path to project docker binaries folder")
   
args = ap.parse_args()

print(nwn_destination_hd0)
if None not in (args.binary, nwn_destination_hd0):
    nwn_destination_bin = os.path.join(nwn_destination_hd0 + "\\dotnet")
    os.system("attrib -r \"" + nwn_destination_bin +"\\*.*\" /s /d")
    copy_folder_content(args.binary, nwn_destination_bin)
    
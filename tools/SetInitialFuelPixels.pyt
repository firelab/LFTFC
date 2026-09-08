import os
import sys
import sqlite3
import sqlalchemy # as sqa
import pandas as pd
import numpy as np
import arcpy
import time
import ctypes
import pathlib
import re

from sqlalchemy import create_engine, text, inspect, select
from sqlalchemy.engine import URL
from sqlalchemy.orm import Session
from sqlalchemy import MetaData, Table
from contextlib import closing
from pathlib import Path
#from numpy import nan

#from dataclasses import dataclass, field
from typing import Any, Optional, List, Dict, Tuple

import site
import subprocess

class Toolbox(object):
    def __init__(self):
        self.label = "Toolbox Name"
        self.alias = "Toolbox Alias"
        self.tools = [Rules_Setup, Clear_Selected_MU, export_sqlite_to_csv, import_csv_to_sqlite]

class Rules_Setup(object):
    def __init__(self):
        self.label = "Set Initial Pixel Count"
        self.description = "Description"
        self.canRunInBackground = False

    def getParameterInfo(self):

        proj_path = arcpy.Parameter(
            displayName="Selected Project Path",
            name="in_projpath",
            datatype="DEFolder",
            parameterType="Required",
            direction="Input"
        )
        mu = arcpy.Parameter(
            displayName="Selected MU",
            name="in_mu",
            datatype="GPString",
            parameterType="Required",
            direction="Input"
        )

        proj_path.value = r"E:\e_GIS\LFTFC\Test_Data_Set"
        mu.value = "G_TestData_MU"

        return [proj_path, mu]

    def isLicensed(self):
        return True

    def updateParameters(self, parameters):
        return

    def updateMessages(self, parameters):
        if parameters[0].value:
            try:
                db_url = f"sqlite:///{parameters[0].ValueAsText}//LFTFC_new.sqlite"
                engine = create_engine(db_url)
                metadata = MetaData()
                namtab = Table("DATA_MU_Name", metadata, autoload_with=engine)
                with engine.connect() as conn:
                    statement = sqlalchemy.select(namtab.c.Name)
                    tablesall = conn.execute(statement).all()
                    tables = [row[0] for row in tablesall]
                    tables.sort()
                engine.dispose()
                parameters[1].filter.list = tables
            except Exception as e:
                arcpy.AddWarning(f"Could not read database: {e}")
                parameters[1].filter.list = []
        return

    def execute(self, parameters, messages):
        proj_path = parameters[0].valueAsText
        mu = parameters[1].valueAsText
        
        count_pixels(proj_path, mu)

class Clear_Selected_MU(object):
    def __init__(self):
        self.label = "Clear Selected MU Initial Pixel Count"
        self.description = "Description"
        self.canRunInBackground = False

    def getParameterInfo(self):

        proj_path = arcpy.Parameter(
            displayName="Selected Project Path",
            name="in_projpath",
            datatype="DEFolder",
            parameterType="Required",
            direction="Input"
        )
        mu = arcpy.Parameter(
            displayName="Selected MU",
            name="in_mu",
            datatype="GPString",
            parameterType="Required",
            direction="Input"
        )

        proj_path.value = r"E:\e_GIS\LFTFC\Test_Data_Set"
        mu.value = "G_TestData_MU"

        return [proj_path, mu]


    def isLicensed(self):
        return True

    def updateParameters(self, parameters):
        if parameters[0].value:
            try:
                db_url = f"sqlite:///{parameters[0].ValueAsText}//LFTFC_new.sqlite"
                engine = create_engine(db_url)
                metadata = MetaData()
                namtab = Table("DATA_MU_Name", metadata, autoload_with=engine)
                with engine.connect() as conn:
                    statement = sqlalchemy.select(namtab.c.Name)
                    tablesall = conn.execute(statement).all()
                    tables = [row[0] for row in tablesall]
                    tables.sort()
                engine.dispose()
                parameters[1].filter.list = tables
            except Exception as e:
                arcpy.AddWarning(f"Could not read database: {e}")
                parameters[1].filter.list = []
        return

    def updateMessages(self, parameters):
        return

    def execute(self, parameters, messages):
        proj_path = parameters[0].valueAsText
        mu = parameters[1].valueAsText

        clear_mu(proj_path, mu)

class export_sqlite_to_csv(object):
    def __init__(self):
        self.label = "Export SQLite database to csv files"
        self.description = "Description"
        self.canRunInBackground = False

    def getParameterInfo(self):

        proj_path = arcpy.Parameter(
            displayName="Selected Project Path",
            name="in_projpath",
            datatype="DEFolder",
            parameterType="Required",
            direction="Input"
        )
        dbtab = arcpy.Parameter(
            displayName="Selected SQLite tables",
            name="in_sql",
            datatype="GPString",
            parameterType="Required",
            direction="Input",
            multiValue=True
        )

        proj_path.value = r"E:\e_GIS\LFTFC\Test_Data_Set"
        dbtab.filter.type = "ValueList"

        return [proj_path, dbtab]


    def isLicensed(self):
        return True

    def updateParameters(self, parameters):
        if parameters[0].value:
            try:
                db_url = f"sqlite:///{parameters[0].ValueAsText}//LFTFC_new.sqlite"
                engine = create_engine(db_url)
                inspector = inspect(engine)
                tables = inspector.get_table_names()
                parameters[1].filter.list = tables
            except Exception as e:
                arcpy.AddWarning(f"Could not read database: {e}")
                parameters[1].filter.list = []
        return

    def updateMessages(self, parameters):
        return

    def execute(self, parameters, messages):
        proj_path = parameters[0].valueAsText
        dbtab = parameters[1].valueAsText

        sqlite_to_csv(proj_path, dbtab)

class import_csv_to_sqlite(object):
    def __init__(self):
        self.label = "Import csv files to SQLite database"
        self.description = "Description"
        self.canRunInBackground = False

    def getParameterInfo(self):

        proj_path = arcpy.Parameter(
            displayName="Selected Project Path",
            name="in_projpath",
            datatype="DEFolder",
            parameterType="Required",
            direction="Input"
        )
        csv = arcpy.Parameter(
            displayName="Selected CSV Files",
            name="in_csv",
            datatype="GPString",
            parameterType="Required",
            direction="Input",
            multiValue=True
        )

        return [proj_path, csv]


    def isLicensed(self):
        return True

    def updateParameters(self, parameters):
        if parameters[0].value:
            try:
                csvfolder = Path(os.path.join(parameters[0].ValueAsText, "csv_files"))
                csv_files = [file.name for file in csvfolder.glob('*.csv')]
                parameters[1].filter.list = csv_files
            except Exception as e:
                arcpy.AddWarning(f"Could not read database: {e}")
                parameters[1].filter.list = []
        return

    def updateMessages(self, parameters):
        return

    def execute(self, parameters, messages):
        proj_path = parameters[0].valueAsText
        csv = parameters[1].valueAsText

        csv_to_sqlite(proj_path, csv)

def pip(module_name, package_name=None):

    pip = 'C:/Program Files/ArcGIS/Pro/bin/Python/envs/arcgispro-py3/Scripts/pip.exe'
    install_name = package_name if package_name else module_name
    subprocess.run([pip, 'install', '--user', install_name], shell=True)
    
    if site.USER_SITE not in sys.path:
        sys.path.append(site.USER_SITE)

def build_sqlite_engine(db_path: str):
    """
    Create a SQLAlchemy engine for SQLite.
    Example: db_path = r"C:\path\to\rules.db"
    """
    return create_engine(f"sqlite:///{db_path}", echo=False)

def _is_numeric_str(s: Any) -> bool:
    if s is None:
        return False
    try:
        float(str(s).strip())
        return True
    except Exception:
        return False

def _to_int(s: Any) -> int:
    if s is None or s == "" or pd.isna(s):
        return 0
    try:
        return int(float(str(s).strip()))
    except Exception:
        return 0
    
def _evt_dist_key(evt: int, dist: int) -> str:
    # makes concatenated key of evt and dist
    return f"{int(evt)}{int(dist)}"

def load_master_frames(cmb_df: pd.DataFrame, rules_df: pd.DataFrame) -> Tuple[pd.DataFrame, pd.DataFrame]:
    
    """
    Read all rows from CMB and Ruleset once into pandas DataFrames.
    Also build EVT&DIST totals dictionary used to compute percent (EvtPer).
    """
    def prep_cmb_df(cmb_df, df_rules):
        # CMB
        rules_unique = df_rules.drop_duplicates(subset=['EVT', 'DIST'])
        cmb_fields = ['EVTR', 'DIST', 'EVCR', 'EVHR', 'BPSRF', 'WILDCARD', 'COUNT']
        df_cmb = cmb_df[cmb_fields].copy()
        # df_cmb = pd.read_sql_query(
        #     text(f'SELECT EVTR, DIST, EVCR, EVHR, BPSRF, WILDCARD, "COUNT" AS COUNT FROM {cmb_table}'), conn)
        # set specific column types
        df_cmb["EVTR"] = df_cmb["EVTR"].astype(int)
        df_cmb["DIST"] = df_cmb["DIST"].astype(int)
        df_cmb["EVCR"] = df_cmb["EVCR"].astype(int)
        df_cmb["EVHR"] = df_cmb["EVHR"].astype(int)
        df_cmb["BPSRF"] = df_cmb["BPSRF"].astype('Int64')
        df_cmb["WILDCARD"] = df_cmb["WILDCARD"].astype(str)
        df_cmb["COUNT"] = df_cmb["COUNT"].astype(int)
        new_cmb = df_cmb.merge(rules_unique[['EVT','DIST']], left_on=['EVTR', 'DIST'], right_on=['EVT', 'DIST'], how='inner')

        return new_cmb

    def prep_rules_df(rules_df):
        # Ruleset
        rules_fields_str = ("ID, EVT, DIST, Cover_Low, Cover_High, Height_Low, " 
                        "Height_High, BPSRF, Wildcard, FBFM13, FBFM40, CanFM, " 
                        "FCCS, FLM, CCover, CHeight, CBD13x100, CBD40x100, "
                        "CBH13mx10, CBH40mx10, Canopy, OnOff, Notes, PixelCount")
        rules_fields = rules_fields_str.split(", ")
        df_rules_full = rules_df[rules_fields].copy()

        df_rules = df_rules_full[((df_rules_full['OnOff'] == 'On') & (df_rules_full['PixelCount'] == '')) | (df_rules_full['PixelCount'].isnull())]

        # set specific column types
        for col in ["ID", "EVT", "DIST", "Cover_Low", "Cover_High", "Height_Low", "Height_High",
                    "FBFM13", "FCCS", "FLM", "CCover", "CHeight", "CBH13mx10", "CBH40mx10", "Canopy"]:
            df_rules[col] = df_rules[col].astype(int)
        df_rules["CBD13x100"] = df_rules["CBD13x100"].astype(float)
        df_rules["CBD40x100"] = df_rules["CBD40x100"].astype(int)
        df_rules["BPSRF"] = df_rules["BPSRF"].astype(str)
        df_rules["Wildcard"] = df_rules["Wildcard"].astype(str)
        df_rules["OnOff"] = df_rules["OnOff"].astype(str)
        df_rules["PixelCount"] = pd.to_numeric(df_rules["PixelCount"], errors="coerce").astype("Int64")
        df_rules['PixelCount'] = ""
        return df_rules
    
    df_rules = prep_rules_df(rules_df)
    df_cmb = prep_cmb_df(cmb_df, df_rules)

    return df_cmb, df_rules

def get_evtPer_totals(df_cmb):
    # Totals per (EVTR, DIST) — used for EvtPer in output
    totals = (
        df_cmb.groupby(["EVTR", "DIST"], as_index=False)["COUNT"]
        .sum()
        .rename(columns={"COUNT": "TotalCount"})
    )
    totals_dict = { _evt_dist_key(row.EVTR, row.DIST): int(row.TotalCount) for _, row in totals.iterrows() }

    return totals_dict

def mask_for_rule(r: pd.Series, df_cmb_g: pd.DataFrame) -> pd.Series:
    """
    Build the predicate mask for a single rule r over df_cmb_g rows
    of the same (EVTR, DIST). Mirrors VB semantics including 'any'.
    """
    cov_mask = df_cmb_g["EVCR"].between(int(r["Cover_Low"]), int(r["Cover_High"]))
    hgt_mask = df_cmb_g["EVHR"].between(int(r["Height_Low"]), int(r["Height_High"]))

    # BPSRF — Ruleset can hold 'any' or numeric-like string; CMB holds int
    bps_field = str(r["BPSRF"])
    if bps_field != "any" and _is_numeric_str(bps_field):
        bps_val = int(float(bps_field))
        bps_mask = (df_cmb_g["BPSRF"] == bps_val)
    else:
        bps_mask = pd.Series(True, index=df_cmb_g.index)

    # Wildcard — Ruleset Wildcard vs CMB WILDCARD
    wc_field = str(r["Wildcard"])
    if wc_field != "any":
        wc_mask = (df_cmb_g["WILDCARD"] == wc_field)
    else:
        wc_mask = pd.Series(True, index=df_cmb_g.index)

    return cov_mask & hgt_mask & bps_mask & wc_mask

def make_null_rule_df(df_rules_g: pd.DataFrame) -> pd.DataFrame:
    """
    Create a single-row DataFrame that matches df_rules_g.columns and dtypes,
    representing the VB 'null rule' placeholder (ID==0, 'any' filters, 100 bounds).
    """
    if df_rules_g.empty:
        # Fallback minimal structure (shouldn't happen in normal flow)
        cols = ["ID","EVT","DIST","Cover_Low","Cover_High","Height_Low","Height_High",
                "BPSRF","Wildcard","OnOff","PixelCount_new"]
        d = {c: np.nan for c in cols}
        d.update({"ID":0,"EVT":0,"DIST":0,"Cover_Low":100,"Cover_High":100,"Height_Low":100,"Height_High":100,
                  "BPSRF":"any","Wildcard":"any","OnOff":"", "PixelCount_new":None}) #0.0})
        return pd.DataFrame([d])
    # Build a defaults dict with all columns present
    defaults = {}
    for c in df_rules_g.columns:
        if c == "ID":            defaults[c] = 0
        elif c == "EVT":         defaults[c] = int(df_rules_g["EVT"].iloc[0])
        elif c == "DIST":        defaults[c] = int(df_rules_g["DIST"].iloc[0])
        elif c == "Cover_Low":   defaults[c] = 100
        elif c == "Cover_High":  defaults[c] = 100
        elif c == "Height_Low":  defaults[c] = 100
        elif c == "Height_High": defaults[c] = 100
        elif c == "BPSRF":       defaults[c] = "any"
        elif c == "Wildcard":    defaults[c] = "any"
        elif c == "OnOff":       defaults[c] = ""
        elif c == "PixelCount_new": defaults[c] = 0.0
        else:
            # keep something benign for unused fields
            defaults[c] = df_rules_g[c].iloc[0] if len(df_rules_g[c].dropna()) else np.nan

    null_df = pd.DataFrame([defaults])
    # Coerce dtypes to match (especially ensures float for PixelCount_new)
    for c in df_rules_g.columns:
        try:
            null_df[c] = null_df[c].astype(df_rules_g[c].dtype)
        except Exception:
            pass
    return null_df

def compute_base_pixel_counts(df_cmb_g: pd.DataFrame, df_rules_g: pd.DataFrame) -> pd.DataFrame:
    """
    For one (EVT, DIST) group, compute base PixelCount for each rule using masks.
    - On rules: sum of COUNT under the mask (Int64)
    - Off rules: <NA> (nullable integer)
    Returns df_rules_g with 'PixelCount_new' (Int64).
    """
    out = df_rules_g.copy()
    # initialize as nullable integer
    out["PixelCount_new"] = pd.array([pd.NA] * len(out), dtype="Int64")

    for i, r in out.iterrows():
        if r["OnOff"] == "On":
            m = mask_for_rule(r, df_cmb_g)
            if not m.empty:
                cnt = int(df_cmb_g.loc[m, "COUNT"].sum())
                out.at[i, "PixelCount_new"] = cnt
            else:
                out.at[i, "PixelCount_new"] = pd.NA 
        else:
            out.at[i, "PixelCount_new"] = pd.NA

    return out

def finalize_acres_percent(df_rules_g: pd.DataFrame, total_px: int) -> pd.DataFrame:
    """
    Compute Acres and EvtPer from PixelCount_new for notebook display only.
    Off rules (NaN PixelCount_new) get blank acres/percent strings.
    """
    df_rules_g = df_rules_g.copy()

    def acres_from_px(px):
        if pd.isna(px):
            return ""
        return f"{round(int(px) * 0.2223948, 2)}"

    def percent_from_px(px):
        if pd.isna(px) or total_px <= 0:
            return ""
        return f"{round(int(px) / total_px * 100.0, 2)}%"

    df_rules_g["Acres_new"] = df_rules_g["PixelCount_new"].apply(acres_from_px)
    #df_rules_g["EvtPer_new"] = df_rules_g["PixelCount_new"].apply(percent_from_px)
    return df_rules_g

def create_mask(rule_row: pd.Series, df_cmb_g: pd.DataFrame) -> pd.Series:
    """
    Equivalent to VB CreateSQL(...) but as a pandas mask.
    If rule_row is a placeholder (ID==0), return True mask (no constraint).
    """
    if int(rule_row["ID"]) == 0:
        return pd.Series(True, index=df_cmb_g.index)
    return mask_for_rule(rule_row, df_cmb_g)

def overlap_sum(varC: pd.Series, varE: pd.Series, varW: pd.Series, varEW: pd.Series,
                df_cmb_g: pd.DataFrame) -> int:
    """
    Mirror VB RuleOverLap(...) branches using preloaded CMB slice (df_cmb_g).
    Returns integer sum of COUNT under combined masks.
    """
    idC, idE, idW, idEW = int(varC["ID"]), int(varE["ID"]), int(varW["ID"]), int(varEW["ID"])

    def sum_for(pred_masks: List[pd.Series]) -> int:
        m = pred_masks[0]
        for p in pred_masks[1:]:
            m = m & p
        return int(df_cmb_g.loc[m, "COUNT"].sum())

    # Branches
    if idC and idE and idW and idEW:
        # Special case: only count if E.BPS != EW.BPS AND W.Wildcard != EW.Wildcard
        if str(varE["BPSRF"]) != str(varEW["BPSRF"]) and str(varW["Wildcard"]) != str(varEW["Wildcard"]):
            return sum_for([create_mask(varEW, df_cmb_g), create_mask(varW, df_cmb_g),
                            create_mask(varE, df_cmb_g), create_mask(varC, df_cmb_g)])
        else:
            return 0

    elif idC and (not idE) and idW and idEW:
        return sum_for([create_mask(varEW, df_cmb_g), create_mask(varW, df_cmb_g), create_mask(varC, df_cmb_g)])

    elif idC and idE and (not idW) and idEW:
        return sum_for([create_mask(varEW, df_cmb_g), create_mask(varE, df_cmb_g), create_mask(varC, df_cmb_g)])

    elif idC and idE and idW and (not idEW):
        return sum_for([create_mask(varW, df_cmb_g), create_mask(varE, df_cmb_g), create_mask(varC, df_cmb_g)])

    elif idC and idE and (not idW) and (not idEW):
        return sum_for([create_mask(varE, df_cmb_g), create_mask(varC, df_cmb_g)])

    elif idC and (not idE) and idW and (not idEW):
        return sum_for([create_mask(varW, df_cmb_g), create_mask(varC, df_cmb_g)])

    elif idC and (not idE) and (not idW) and idEW:
        return sum_for([create_mask(varEW, df_cmb_g), create_mask(varC, df_cmb_g)])

    elif (not idC) and idE and idW and idEW:
        if str(varE["BPSRF"]) != str(varEW["BPSRF"]) and str(varW["Wildcard"]) != str(varEW["Wildcard"]):
            return sum_for([create_mask(varEW, df_cmb_g), create_mask(varW, df_cmb_g), create_mask(varE, df_cmb_g)])
        else:
            return 0

    elif (not idC) and idE and (not idW) and idEW:
        return sum_for([create_mask(varEW, df_cmb_g), create_mask(varE, df_cmb_g)])

    elif (not idC) and idE and idW and (not idEW):
        return sum_for([create_mask(varW, df_cmb_g), create_mask(varE, df_cmb_g)])

    elif (not idC) and (not idE) and idW and idEW:
        return sum_for([create_mask(varEW, df_cmb_g), create_mask(varW, df_cmb_g)])

    else:
        return 0

def apply_overlap_corrections(df_cmb_g: pd.DataFrame, df_rules_g: pd.DataFrame, outtxt, debug: bool = False) -> pd.DataFrame:
    """
    Reproduce VB nested loop logic over C/E/W/EW rule buckets and adjust PixelCount_new accordingly.
    Uses a column-aligned null rule to avoid concat/dtype warnings.
    """
    out = df_rules_g.copy()

    # Buckets: only 'On' rules are considered in overlap correction
    colC  = out[(out["OnOff"] == "On") & (out["BPSRF"] == "any") & (out["Wildcard"] == "any")].copy()
    colE  = out[(out["OnOff"] == "On") & (out["BPSRF"] != "any") & (out["Wildcard"] == "any")].copy()
    colW  = out[(out["OnOff"] == "On") & (out["BPSRF"] == "any") & (out["Wildcard"] != "any")].copy()
    colEW = out[(out["OnOff"] == "On") & (out["BPSRF"] != "any") & (out["Wildcard"] != "any")].copy()

    null_rule_df = make_null_rule_df(out)

    # Ensure non-empty buckets by appending null when needed (exactly like VB placeholders)
    if colEW.empty: colEW = pd.concat([colEW, null_rule_df], ignore_index=True)
    if colW.empty:  colW  = pd.concat([colW,  null_rule_df], ignore_index=True)
    if colE.empty:  colE  = pd.concat([colE,  null_rule_df], ignore_index=True)

    if debug:
        append_to_txt(outtxt, f"\nBuckets: C={len(colC)}, E={len(colE)}, W={len(colW)}, EW={len(colEW)}")
        #print(f"Buckets: C={len(colC)}, E={len(colE)}, W={len(colW)}, EW={len(colEW)}")

    # --- Pass over C rules ---
    for _, thingC in colC.iterrows():
        for _, thingE in colE.iterrows():
            for _, thingW in colW.iterrows():
                for _, thingEW in colEW.iterrows():
                    delta = overlap_sum(thingC, thingE, thingW, thingEW, df_cmb_g)
                    cond_add = (int(thingE["ID"]) != 0 and int(thingW["ID"]) != 0) or \
                               (int(thingE["ID"]) != 0 and int(thingEW["ID"]) != 0) or \
                               (int(thingW["ID"]) != 0 and int(thingEW["ID"]) != 0)
    
                    # Get base safely (row must exist; ID unique in Ruleset)
                    base_series = out.loc[out["ID"] == int(thingC["ID"]), "PixelCount_new"]
                    
                    if base_series.empty or pd.isna(base_series.iloc[0]):
                        continue
                    
                    base = _to_int(base_series.iloc[0])
                    new_px = base + delta if cond_add else base - delta
                    out.loc[out["ID"] == int(thingC["ID"]), "PixelCount_new"] = float(new_px)

    # --- Pass over E rules ---
    # VB “remove last from E”; emulate by dropping last row when there’s >1
    colE_iter = colE.iloc[:-1] if len(colE) > 1 else colE.iloc[0:0]

    for _, thingE in colE_iter.iterrows():
        for _, thingW in colW.iterrows():
            for _, thingEW in colEW.iterrows():
                delta = overlap_sum(null_rule_df.iloc[0], thingE, thingW, thingEW, df_cmb_g)
                cond_add = (int(thingW["ID"]) != 0 and int(thingEW["ID"]) != 0)

                base_series = out.loc[out["ID"] == int(thingE["ID"]), "PixelCount_new"]
                if base_series.empty or pd.isna(base_series.iloc[0]):
                    continue
                base = _to_int(base_series.iloc[0])
                new_px = base + delta if cond_add else base - delta
                out.loc[out["ID"] == int(thingE["ID"]), "PixelCount_new"] = float(new_px)

    # --- Pass over W rules ---
    colW_iter = colW.iloc[:-1] if len(colW) > 1 else colW.iloc[0:0]

    for _, thingW in colW_iter.iterrows():
        for _, thingEW in colEW.iterrows():
            delta = overlap_sum(null_rule_df.iloc[0], null_rule_df.iloc[0], thingW, thingEW, df_cmb_g)

            base_series = out.loc[out["ID"] == int(thingW["ID"]), "PixelCount_new"]
            if base_series.empty or pd.isna(base_series.iloc[0]):
                continue
            base = _to_int(base_series.iloc[0])
            new_px = base - delta
            out.loc[out["ID"] == int(thingW["ID"]), "PixelCount_new"] = float(new_px)
    
    out["PixelCount_new"] = out["PixelCount_new"].astype("Int64")
    return out

def build_updates(df_rules_all: pd.DataFrame) -> List[Dict[str, Any]]:
    """
    Build parameter dicts for batch UPDATE of PixelCount only.
    """
    df_rules_all["PixelCount"] = pd.to_numeric(df_rules_all["PixelCount"], errors="coerce").astype("Int64")
    df_rules_all["PixelCount"] = df_rules_all["PixelCount_new"]
    updates = []
    for _, r in df_rules_all.iterrows():
        new_px = r["PixelCount"]
        updates.append({
            "pixel_count": "" if pd.isna(new_px) else int(new_px),
            "id": int(r["ID"])
        })
       
        # orig_px = r["PixelCount"]  # Int64 or <NA>
        # new_px  = r["PixelCount_new"]  # Int64 or <NA>

        # # Find changes in pixels
        # changed = (pd.isna(orig_px) and not pd.isna(new_px)) or \
        #           (not pd.isna(orig_px) and pd.isna(new_px)) or \
        #           (not pd.isna(orig_px) and not pd.isna(new_px) and float(orig_px) != float(new_px))
        
        # if changed:
        #     updates.append({
        #         "pixel_count": None if pd.isna(new_px) else int(new_px),
        #         "id": int(r["ID"])
        #     })
    return updates

def batch_update_rules(conn, rules_table: str, updates: List[Dict[str, Any]]) -> int:
    """
    Execute a single batch update for all changed rows.
    """
    if not updates:
        return 0
    sql = text(f"UPDATE {rules_table} SET PixelCount = :pixel_count WHERE ID = :id")
    conn.execute(sql, updates)
    return len(updates)

def append_to_txt(filename, new_row_data):
    with open(filename, 'a') as f:
        f.write(new_row_data)

def create_sql_engine(in_sqlite: str):

    sql_engine_path = f"sqlite:///{in_sqlite}"
    return create_engine(sql_engine_path, echo=False)

def full_query(mu):
    rulesR = f"{mu}_Rulesets"
    comboR = f"{mu}_CMB"

    return f"""
        SELECT COUNT(*) AS MatchCount
        FROM (
            SELECT
                r.EVT,
                r.DIST,
                r.OnOff,
                r.PixelCount
            FROM {comboR} AS c
            INNER JOIN {rulesR} AS r
                ON c.DIST = r.DIST
                AND c.EVTR = r.EVT
            GROUP BY
                r.EVT,
                r.DIST,
                r.OnOff,
                r.PixelCount
            HAVING
                ((r.OnOff='On' AND r.PixelCount=''))
                OR (r.PixelCount IS NULL)
        ) AS sub;
    """

def run_check_count_sql(db, query):
    engine = create_sql_engine(db)
    
    with engine.connect() as conn:
        value = conn.execute(text(query)).scalar()

    engine.dispose()
    return value

def run_full_inmemory_update(db_info, outtxt, use_height_sort: bool = False, debug: bool = False) -> pd.DataFrame:    
    """
    1) Open engine & connection
    2) Read CMB and Ruleset once
    3) For each (EVT,DIST) group: compute base counts + overlap corrections in memory
    4) Compute Acres/EvtPer
    5) Batch update PixelCount once; commit
    6) Return DataFrame for inspection/export
    """
    # check should only run on rules where OnOff = "On" and PixelCount = ''  - or - PixelCount Is Null
    arcpy.AddMessage("At the beginning of main run")
    print("At the beginning of main run")

    # works with sqlite db
    engine = create_sql_engine(db_info.sql_path)
    arcpy.AddMessage("after engine build")
    print("after engine build")
    
    cmb_table = pd.read_sql(db_info.sql_query_p, engine)
    rules_table = pd.read_sql(db_info.sql_query_r, engine)
    arcpy.AddMessage(f"cmb length original is: {len(cmb_table)}")
    arcpy.AddMessage(f"rules length original is: {len(rules_table)}")
    
    with engine.connect() as conn:
        trans = conn.begin()
        df_rules_all = None
        try:
            # original run with connection to db
            df_cmb, df_rules = load_master_frames(cmb_table, rules_table)

            # if df_cmb len is 0, skip to end            
            if len(df_cmb) > 0:
                # totals_dict = get_evtPer_totals(df_cmb)

                if debug:
                    append_to_txt(outtxt, f"\nLoaded CMB rows: {len(df_cmb)}, Rules rows: {len(df_rules)}")
                    print(f"Loaded CMB rows: {len(df_cmb)}, Rules rows: {len(df_rules)}")


                # Optional sort (output only)
                order_cols = ["OnOff", "BPSRF", "Wildcard"] + (
                    ["Height_Low", "Cover_Low"] if use_height_sort else ["Cover_Low", "Height_Low"]
                )

                results = []
                group_count = 0

                # Iterate by (EVT,DIST)
                for (evt, dist), df_rules_g in df_rules.groupby(["EVT", "DIST"], as_index=False):
                    df_cmb_g = df_cmb[(df_cmb["EVTR"] == int(evt)) & (df_cmb["DIST"] == int(dist))].copy()
                    total_px = int(df_cmb_g["COUNT"].sum()) if not df_cmb_g.empty else "" #0

                    if debug:
                        append_to_txt(outtxt, f"\nGroup EVT={evt}, DIST={dist}, CMB rows={len(df_cmb_g)}, total_px={total_px}")
                        #print(f"Group EVT={evt}, DIST={dist}, CMB rows={len(df_cmb_g)}, total_px={total_px}")

                    # Base counts
                    df_rules_g = compute_base_pixel_counts(df_cmb_g, df_rules_g)

                    # Overlap corrections
                    df_rules_g = apply_overlap_corrections(df_cmb_g, df_rules_g, outtxt, debug=debug)
                    
                    results.append(df_rules_g)
                    group_count += 1
                
                if results:
                    df_rules_all = pd.concat(results, ignore_index=True)

                    df_rules_all["PixelCount_new"] = df_rules_all["PixelCount_new"].astype("Int64")
                    
                    df_rules_all = df_rules_all.sort_values(order_cols, ascending=[False, False, False, True, True])
                else:
                    # No groups → return the (possibly empty) rules frame with the new columns added
                    df_rules["PixelCount_new"] = np.nan
                    df_rules["Acres_new"] = ""
                    df_rules["EvtPer_new"] = ""
                    df_rules_all = df_rules

                # Build updates and write to sqlite testing db
                updates = build_updates(df_rules_all)
                updated_rows = batch_update_rules(conn, db_info.comboR, updates)
                arcpy.AddMessage(f"number of rows updated: {updated_rows}")

                trans.commit()
                #if debug:
                    #print(f"Processed {group_count} (EVT,DIST) groups. Updated {updated_rows} rows.")
                #return df_rules_all

        except Exception as ex:
            trans.rollback()
            # Surface the exact error so we can see it in the notebook
            raise RuntimeError(f"run_full_inmemory_update failed: {ex}") from ex

def format_seconds(seconds):
    """Converts a duration in seconds to H:M:S format."""
    hours, rem = divmod(seconds, 3600)
    minutes, seconds = divmod(rem, 60)
    return f"{int(hours):02}:{int(minutes):02}:{seconds:05.2f}"

class DBInfo():

    def __init__(self, proj_path, mu):
        self.mu = mu
        self.mdb_name = "LF_TFC_Toolbar.mdb"
        self.sql_name = "LFTFC_new.sqlite"
        self.comboR = f"{mu}_Rulesets"
        self.comboP = f"{mu}_CMB"
        self.sql_query_r = f"SELECT * FROM {self.comboR}"
        self.sql_query_p = f"SELECT * FROM {self.comboP}"
        self.db_path = os.path.join(proj_path, self.mdb_name)
        self.sql_path = os.path.join(proj_path, self.sql_name)

def popup_message(msg):
    ctypes.windll.user32.MessageBoxW(0, msg, "Toolbox info", 0)

def count_pixels(proj_path, mu):
    outtxt = "" 
 
    db_info = DBInfo(proj_path, mu)
    strSQLcount = full_query(db_info.mu)   

    # use sql query prior to loading dataframes
    empty_rows_count = run_check_count_sql(db_info.sql_path, strSQLcount)

    arcpy.AddMessage(f"empty rows count: {empty_rows_count}")    

    if empty_rows_count > 0:
        df_out = run_full_inmemory_update(db_info, outtxt, use_height_sort=False, debug=False)

def clear_mu(proj_path, mu):
    db_info = DBInfo(proj_path, mu)

    engine = create_sql_engine(db_info.sql_path)

    with engine.connect() as conn:
        stmt = text(f"UPDATE {db_info.comboR} SET PixelCount = ''")
        conn.execute(stmt)
        conn.commit()

def sqlite_to_csv(proj_path, dbtables):
    db_info = DBInfo(proj_path, "Test")

    # works with sqlite db
    engine = create_sql_engine(db_info.sql_path)

    csvfolder = os.path.join(proj_path, "csv_files")
    Path(csvfolder).mkdir(parents=True, exist_ok=True)
    try:
        with engine.connect() as conn: 
            inspector = inspect(conn)
            sql_table_names = inspector.get_table_names()

            for table in dbtables.split(";"):
                if table in sql_table_names:
                    df = pd.read_sql_table(table, conn)
                    df.to_csv(os.path.join(csvfolder, f"{table}.csv"))
                else:
                    arcpy.AddMessage(f"table: {table} not in sqlite db")
    finally:
        engine.dispose()

def csv_to_sqlite(proj_path, dbtables):
    db_info = DBInfo(proj_path, "Test")
    
    # works with sqlite db
    engine = create_sql_engine(db_info.sql_path)

    try:
        with engine.begin() as conn:
            for csvfile in dbtables.split(";"):
                csvname = Path(csvfile).stem

                df = pd.read_csv(os.path.join(proj_path, 'csv_files', csvfile))
                df.to_sql(csvname, conn, if_exists='replace', index=False)   
    finally:
        engine.dispose()
import React from 'react';
import {useContext, useState, useEffect} from 'react';
import axios from 'axios';
import { json, useNavigate, useParams } from 'react-router-dom';
import ReactJson from 'react-json-view'

const ModuleJSONData = ({initialDatapoint, handleDataChange}) => {
    const sectionName = initialDatapoint.sectionName;
    const isEditable = initialDatapoint.isDataEditable;
    const handleJsonChange =(e)=>{
        handleDataChange(sectionName, e.updated_src);
    }
    return (
             <div>
               <h3>Section: {sectionName}</h3>
               <h5>Profile: {initialDatapoint.contextName}</h5>
               <div>
                { isEditable ? 
                    <ReactJson key={initialDatapoint.sectionName} onEdit={handleJsonChange} onAdd={handleJsonChange} onDelete={handleJsonChange} src={initialDatapoint.content} />
                    :
                    <ReactJson key={initialDatapoint.sectionName} src={initialDatapoint.content} />
                }
                </div>
        
                
            </div>
        )
    };

export default ModuleJSONData;

import React from 'react';
import ReactJson from 'react-json-view';
import {useContext, useState, useEffect} from 'react';
import './ModuleHTML.css';

const ModuleHTML = ({htmlDashboard, datastructure, handleHtmlChange}) => {
    const [edit, setEdit] = useState(false);
    const [inputValue, setInputValue] = useState(htmlDashboard); 
    const [parsedValue, setParsedValue] = useState(htmlDashboard); 

    const replacePlaceholders = (templateString, dataStructure) => {
        return templateString.replace(/{{(.*?)}}/g, (match, placeholder) => {
            const sectionName = placeholder.split('.')[0];
            const section = dataStructure.find(s => s.sectionName === sectionName);
            return getJsonDirective(placeholder.split('.').slice(1), section.content);  
        });
    };
    const getJsonDirective=(keys, section)=>{
        if (keys.length == 0) return section;
        else if (keys.length == 1) return section[keys[0]];
        else{
           return getJsonDirective(keys.slice(1), section[keys[0]])
        }
       
    }
    const changeEditMode= ()=>{
        setEdit(!edit);
        if (edit == true){
            setParsedValue(replacePlaceholders(inputValue, datastructure))
        }
    }
    const updateHtml= ()=>{
        setEdit(false);
        setParsedValue(replacePlaceholders(inputValue, datastructure))
        if (inputValue != htmlDashboard){
            handleHtmlChange(inputValue);
        }
    }
    return (
            <> 
               <input 
                    className="input-button"
                    type="button" 
                    value="Edit"
                    onClick={changeEditMode} 
                />
                <input 
                    className="input-button"
                    type="submit" 
                    value="Save"
                    onClick={updateHtml} 
                />

                {edit ? 
                    <div className="module-html">
                        <textarea 
                            className="input-text"
                            type="text" 
                            value={inputValue} 
                            onChange={(e) => setInputValue(e.target.value)}
                        /> 
                    </div>
                    
                    : 
                    <div className="module-html" dangerouslySetInnerHTML={{ __html: parsedValue }}></div>
                }
        </>
        )
    };

export default ModuleHTML;

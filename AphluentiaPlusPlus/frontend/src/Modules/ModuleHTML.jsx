import React from 'react';
import ReactJson from 'react-json-view';
import {useContext, useState, useEffect} from 'react';
import './ModuleHTML.css';

const ModuleHTML = ({module, handleHtmlChange}) => {
    const [edit, setEdit] = useState(false);
    const [htmlSection, setHtmlSection] = useState(0); // 0 is card
    const [inputValue, setInputValue] = useState(module.moduleData.htmlCard); 
    const [parsedValue, setParsedValue] = useState(module.moduleData.htmlCard); 

    const dataStructure = module.moduleData.dataStructure.filter(data=> 
        data.contextName === module.moduleData.activeContextName
    )
    const replacePlaceholders = (templateString) => {
        return templateString.replace(/{{(.*?)}}/g, (match, placeholder) => {
            const sectionName = placeholder.split('.')[0];
            const section = dataStructure.find(s => s.sectionName === sectionName);
            return getJsonDirective(placeholder.split('.').slice(1), section.content);  
        });
    };
    const replaceCardPlaceholders = (templateString) => {
        return templateString.replace(/{{(.*?)}}/g, (match, placeholder) => {
            return getJsonDirective(placeholder.split('.'), module);  
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
            const value = htmlSection == 0 ? (replaceCardPlaceholders(inputValue)) : (replacePlaceholders(inputValue))
            setParsedValue(value);
            
        }
    }
    const changeBetweenCardAndDashboard= ()=>{
        if (htmlSection == 0){
            setInputValue(module.moduleData.htmlDashboard);
            const value = replacePlaceholders(module.moduleData.htmlDashboard);
            setParsedValue(value);
            setHtmlSection(1);
        }else{
            setInputValue(module.moduleData.htmlCard);
            const value = replaceCardPlaceholders(module.moduleData.htmlCard);
            setParsedValue(value);
            setHtmlSection(0);
        }
    }
    const updateHtml= ()=>{
        setEdit(false); 
        const value = htmlSection == 0 ? (replaceCardPlaceholders(inputValue)) : (replacePlaceholders(inputValue))
        setParsedValue(value);
        if (inputValue != module.moduleData.htmlDashboard && inputValue != module.moduleData.htmlCard){
            handleHtmlChange(inputValue, htmlSection==0 );
        }
    }
    
    useEffect(() => {
        updateHtml();
    }, []);
    return (
            <> <p className="module-html-title">
                    Currently Editing Application's {htmlSection == 0 ? "Card" : "Dashboard"}
                    <input 
                        className="input-button-change"
                        type="button" 
                        value={"Change To Edit "+(htmlSection == 0? "Dashboard" : "Card")}
                        onClick={changeBetweenCardAndDashboard} 
                    />
                </p>
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
        </>
        )
    };

export default ModuleHTML;

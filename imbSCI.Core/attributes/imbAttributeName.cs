// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbAttributeName.cs" company="imbVeles" >
//
// Copyright (C) 2018 imbVeles
//
// This program is free software: you can redistribute it and/or modify
// it under the +terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// <summary>
// Project: imbSCI.Core
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Core.attributes
{
    using imbSCI.Core.data;
    using imbSCI.Core.math.measurement;

    /// <summary>
    /// Ime podešavanja na koje se odnosi imbAttribut
    /// </summary>
    public enum imbAttributeName
    {
        #region ---------- imbHELP

        /// <summary>
        /// Opcioni komentar u menu helpu
        /// </summary>
        menuHelp,

        /// <summary>
        /// Tips on using the object
        /// </summary>
        helpTips,

        /// <summary>
        /// Alias for menuHelp
        /// </summary>
        helpDescription,

        /// <summary>
        /// Alias for DisplayName and menuCommandName
        /// </summary>
        helpTitle,

        /// <summary>
        /// Explaining the purpose of the object - why it's used
        /// </summary>
        helpPurpose,

        #endregion ---------- imbHELP

        #region NEURAL NETWORK mapper

        /// <summary>
        /// naziv propertija sa kolekcijom u odnosu na koju postoji relacija
        /// </summary>
        relatedTo,

        /// <summary>
        /// Koji PRoperty da koristi
        /// </summary>
        relationKeyProperty,

        #endregion NEURAL NETWORK mapper

        #region NEURAL NETWORK mapper

        /// <summary>
        /// Property predstavlja izlaznu vrednost za neuronsku mrezu, u msg staviti naziv neurona bez prefiksa
        /// </summary>
        neuralOutput,

        /// <summary>
        /// Property predstavlja ulaznu vrednost za neuronsku mrezu
        /// </summary>
        neuralInput,

        #endregion NEURAL NETWORK mapper

        #region EVALUATION ATTRIBUTI

        /// <summary>
        /// oznacava da je property predvidjen za evaluaciju, u msg moze da se stavi naziv evaluacije - ako podrzava vise
        /// </summary>
        evaluation,

        /// <summary>
        /// oznacava da su suggested values istovremeno i allowed values
        /// </summary>
        evaluationStrictValues,

        /// <summary>
        /// Podesava ulogu propertija u evaluaciji - drugi parametar je evaluationTask
        /// </summary>
        evaluationPropertyRole,

        //evaluationHelpQuestion,
        //evaluationHelpHints,

        #endregion EVALUATION ATTRIBUTI

        help,

        #region RDF ATTRIBUTI

        /// <summary>
        /// Ime ontološke klase koja je ekvivalent ovoj klasi
        /// </summary>
        rdfClassName,

        /// <summary>
        /// Ime ovog propertija u ontološkoj klasi - ime predikata
        /// </summary>
        rdfPropertyName,

        /// <summary>
        /// Oznacava da je property jedinstveni ID
        /// </summary>
        rdfIdProperty,

        /// <summary>
        /// Podrazumevana putanja za kolekciju na triplestoru
        /// </summary>
        rdfDefaultCollectionPath,

        #endregion RDF ATTRIBUTI

        #region XPATH Attributi

        /// <summary>
        /// Poziva izvrsavanje xPath-a za ovaj properti i smesta rezultat u njega
        /// </summary>
        xmlMapXpath,

        xmlEntityOutput,

        xmlEntityOutputAsString,

        //xmlEntityCollectionOutput,

        /// <summary>
        /// na deklaraciju klase se stavlja,
        /// </summary>
        xmlNodeValueProperty,

        xmlNodeTypeName,

        #endregion XPATH Attributi

        /// <summary>
        /// Marks a property as the cache instance identifier
        /// </summary>
        cacheInstanceId,

        /// <summary>
        /// Iskljucuje podrsku za serijalizaciju
        /// </summary>
        serializationOff,

        serializationOn,

        serializationMode,

        undefined,

        /// <summary>
        /// Proizvoljni string podatak koji se prosledjuje preko atributa
        /// </summary>
        metaData,

        /// <summary>
        /// Iz kog atributa da preuzme vrednost
        /// </summary>
        metaValueFromAttribute,

        #region --- FILE FORMAT

        /// <summary>
        /// Pod direktorijum u kojem ocekuje ove fajlove
        /// </summary>
        fileFormatDefaultSubdir,

        /// <summary>
        /// ekstenzije za fajl format
        /// </summary>
        fileFormatExtensions,

        fileBackupAndRecovery,

        fileFormatForType,

        /// <summary>
        /// Tip enkodiranja koji treba da se koristi
        /// </summary>
        fileEncodingType,

        fileProvideByteOrder,

        ///// <summary>
        ///// Koji imb tip skladisti podatke o ovom fajl formatu
        ///// </summary>
        //fileFormatImbType,
        /// <summary>
        /// Tip koji iscitava podatke
        /// </summary>
        fileFormatReaderType,

        /// <summary>
        /// Tip koji snima podatke
        /// </summary>
        fileFormatWritterType,

        /// <summary>
        /// Tip koji iscitava podatke
        /// </summary>
        fileFormatZippedReaderType,

        /// <summary>
        /// Tip koji snima podatke
        /// </summary>
        fileFormatZippedWritterType,

        #endregion --- FILE FORMAT

        #region ---------- LINK sistem

        /// <summary>
        /// Označava da je property ustvari link ka drugom imbProjectResoruce resursu
        /// </summary>
        link,

        /// <summary>
        /// Oznacava da obelezeni String property predstavlja ustvari ime linkovanog propertija
        /// </summary>
        linkPropertyName,

        /// <summary>
        /// Oznacava ime imbBindable objekta - propertija koji sadrzi objekat iz koga se iscitava vrednost trazenog linka
        /// </summary>
        linkPropertyHost,

        #endregion ---------- LINK sistem

        #region --------- RELATED ITEMS sistem

        /// <summary>
        /// ako ne postoji relacija (tj. upisan id) napravice novu instancu i upisati je u relaciju -- stavlja se na endpoint
        /// </summary>
        relationMakeNewOnNull,

        /// <summary>
        /// ako ne postoji relacija uzece prvi objekat koji ima odgovarajuc backrefference-- stavlja se na endpoint
        /// </summary>
        relationTakeFirstBackrefference,

        /// <summary>
        /// ako ne postoji relacija uzece sve objekte koji imaju odgovarajuc backrefference-- stavlja se na endpoint
        /// </summary>
        relationTakeAllBackrefference,

        /// <summary>
        /// postavice relaciju tako da child unique kolona bude jednaka prosledjenom parent propertiju-- stavlja se na endpoint
        /// </summary>
        relationChildUniqueMatchParentProperty,

        #endregion --------- RELATED ITEMS sistem

        #region -------------- resource selection sistem

        /// <summary>
        /// Da li dozvoljava da budu selektovani pod tipovi?
        /// </summary>
        selectionAllowSubTypes,

        /// <summary>
        /// Ubacuje jedan tip u opcije za selektovanje
        /// </summary>
        selectionAllowType,

        #endregion -------------- resource selection sistem

        #region ----------- CLASS LEVEL ATRIBUTI

        isElementaryModule,

        /// <summary>
        /// Integrisani moduli :: označava da će se integrisani/nestovani modul izvršiti pre parent modula
        /// </summary>
        /// <remarks>
        /// Drugi parametar treba da sadrži prioritet pod kojim će se izvršavati modul.
        /// </remarks>
        /// <example>
        /// Property/modul sa priritetom 1:
        /// <code>
        /// [imb(preProcess, "1")]
        /// </code>
        /// će biti izvršen pre modula za prioritetom 5:
        /// <code>
        /// [imb(preProcess, "5")]
        /// </code>
        /// </example>
        preProcess,

        /// <summary>
        /// Integrisani moduli :: označava da će se integrisani/nestovani modul izvršiti posle parent modula
        /// </summary>
        /// <remarks>
        /// Drugi parametar treba da sadrži prioritet pod kojim će se izvršavati modul.
        /// </remarks>
        /// <example>
        /// Property/modul sa priritetom 1:
        /// <code>
        /// [imb(postProcess, "1")]
        /// </code>
        /// će biti izvršen pre modula za prioritetom 5:
        /// <code>
        /// [imb(postProcess, "5")]
        /// </code>
        /// </example>
        postProcess,

        /// <summary>
        /// definise kako se zove editor - potrebno za editore koji imaju ne standardna imena !
        /// </summary>
        defineEditor,

        #endregion ----------- CLASS LEVEL ATRIBUTI

        #region EDITOR ATTRIBUTI

        /// <summary>
        /// Proslediti mu imbUniversalToolElementMode
        /// </summary>
        editorModulePropertyMode,

        editorRelatedModulesMode,

        editorFlowViewMode,

        /// <summary>
        /// Proslediti mu koji menu
        /// </summary>
        editorHideMenu,

        /// <summary>
        /// Kolike je editor tool velicine
        /// </summary>
        editorToolSize,

        /// <summary>
        /// Postavlja da editor fullscreen -- u property staviti koji display (broj: 1,2,3,4 ili nista za primary)
        /// </summary>
        editorDisplayOption,

        #endregion EDITOR ATTRIBUTI

        /// <summary>
        /// prepravlja data template
        /// </summary>
        dataTemplateOverride,

        /// <summary>
        /// Koristi se za kontesktualne module - nece ih prikazati u meniju
        /// </summary>
        hideInMenu,

        /// <summary>
        /// Definise grupu / putanju u kojoj se nalazi item. Koristi se Group1/Group2 format putanje
        /// </summary>
        menuGroupPath,

        /// <summary>
        /// Oznacava da ce ovaj item imati pod menu sa module types
        /// </summary>
        menuSpecialSubMenu,

        /// <summary>
        /// Koristi se za podesavanje uslova >> ako je ispunjen, onda je menuItem enabled
        /// </summary>
        menuEnabledSwitch,

        /// <summary>
        /// PROSLEDJUJE INFORMACIJU O DINAMICKOM SADRZAJU - nema potrebe!!! ako ima $ us tri
        /// </summary>
        //menuDynamicSubMenu,
        /// <summary>
        /// Dodatni parametar za specijalni pod meni
        /// </summary>
        //   menuSpecialSubMenuParameter,
        /// <summary>
        /// Oznacava da resurs ne moze biti children
        /// </summary>
        notChildrenResource,

        /// <summary>
        /// Traži potvrdu izvršenja
        /// </summary>
        menuAskConfirmation,

        /// <summary>
        /// Definiše da je stavka submenu neke druge stavke - nije implementirano
        /// </summary>
        //  menuSubmenuParent,
        /// <summary>
        /// Ubacuje delimiter pre stavke
        /// </summary>
        menuDelimiter,

        /// <summary>
        /// Greater number, greater pririty. Default value is 100, <see cref="settingsMemberInfoEntry.priority"/>. The same as: <see cref="viewPriority"/>.
        /// </summary>
        menuPriority,

        /// <summary>
        /// Koliko je bitno nesto - od 0 do 4
        /// </summary>
        menuRelevance,

        /// <summary>
        /// Greater number, greater priority. Default value is 100, <see cref="settingsMemberInfoEntry.priority"/>. The same as: <see cref="menuPriority"/>
        /// </summary>
        viewPriority,

        /// <summary>
        /// Sta pise u meniju
        /// </summary>
        menuCommandTitle,

        /// <summary>
        /// Na koji command node sekaci
        /// </summary>
        menuCommandNode,

        /// <summary>
        /// Koji je command key
        /// </summary>
        menuCommandKey,

        /// <summary>
        /// Skracenica na tastaturi
        /// </summary>
        menuCommandKeyboard,

        /// <summary>
        /// Označava da je stavka u meniju važna
        /// </summary>
        menuStyleImportant,

        /// <summary>
        /// Označava da stavka u meniju ključna
        /// </summary>
        menuStyleVIP,

        /// <summary>
        /// Predstavlja ne preporučljivu opciju
        /// </summary>
        menuStyleNotRecommanded,

        /// <summary>
        /// Podesava stil menija preko imena stila
        /// </summary>
        menuStyle,

        #region DIAGNOSTIC

        /// <summary>
        /// Ukljucuje diagnostic mode za modul - prilikom aktiviranja programa otvorice se prozor sa modulom koji ima default vrednosti
        /// </summary>
        diagnosticMode,

        /// <summary>
        /// Postavlja proizvoljnu vrednost na property - ako je izvrseno u diagnostic modu
        /// </summary>
        diagnosticValue,

        /// <summary>
        /// ZA PROPERTY> na oznacen property ucitava debug text content - u msg stavi index diagnosticContent
        /// </summary>
        diagnosticContentText,

        /// <summary>
        /// ZA PROPERTY> na oznacen property ucitava debug preset za odgovarajuci objekat - u msg stavi index preseta (sortirano po datumu pravljenja - DESC)
        /// </summary>
        diagnosticContentPreset,

        /// <summary>
        /// Koji metod da pozove kada se ucita diagnostic windows. Ime metoda iz Editor kontrole.
        /// </summary>
        diagnosticTestMetodInvoke,

        /// <summary>
        /// Poziva Operation unutar modula
        /// </summary>
        diagnosticModuleOperationInvoke,

        /// <summary>
        /// Poziva Operation unutar editora
        /// </summary>
        diagnosticEditorOperationInvoke,

        #endregion DIAGNOSTIC

        /// <summary>
        /// HEX oznakaosnovne boje koja se koristi prilikom vizuelizacije objekta
        /// </summary>
        basicColor,

        /// <summary>
        /// iconKey override - ikona koja se pojavljuje u meniju
        /// </summary>
        menuIcon,

        /// <summary>
        /// Pravilo kako se ponaša u meniju
        /// </summary>
        menuRule_AcceptableType,

        /// <summary>
        /// Pravilo kako se ponasa umeniju
        /// </summary>
        menuRule_AcceptableRole,

        /// <summary>
        /// definise da property predstavlja primary Key - alternativno koristice sql.Unique. Attribut se stavlja: a) ispred propertija koji treba da je key, b) ispred deklaraciuje klase, onda mora da se navede u poruci naziv propertija
        /// </summary>
        collectionPrimaryKey,

        /// <summary>
        /// U kolekciji ce biti iskljucena detekcija primarnog kljuca
        /// </summary>
        collectionDisablePrimaryKey,

        /// <summary>
        /// Podazumevano ime parent membera koji u sebi ima property sa kolekcijom
        /// </summary>
        collectionDefaultParentName,

        /// <summary>
        /// Podazumevano ime membera (property) koji predstavlja collection namenjen smestanju datog tipa
        /// </summary>
        collectionDefaultMemberName,

        /// <summary>
        /// Oznacava metod koji je automatski kreiran, msg treba da sadrzi datum kreiranja
        /// </summary>
        codeCacheMethod,

        measure_setRange,

        measure_setAlarm,

        /// <summary>
        /// The measure meta model name: i.e. cUniqueExternalLinks  from FV64_cUniqueExternalLinks
        /// </summary>
        measure_metaModelName,

        /// <summary>
        /// The measure meta model prefix: i.e. FV64  from FV64_cUniqueExternalLinks
        /// </summary>
        measure_metaModelPrefix,

        measure_important,

        measure_setRole,

        measure_setUnit,

        measure_optimizeUnit,

        measure_calcGroup,

        measure_displayGroup,

        measure_displayGroupDescripton,

        measure_expression,

        measure_operand,

        /// <summary>
        /// This property will be excluded from <see cref="MetricsBase"/> computations
        /// </summary>
        measure_excludeFromMetrics,

        /// <summary>
        /// The measure valuegroup:: defines into what measure parameter this one will be calculated
        /// </summary>
        measure_target,

        /// <summary>
        /// The measure operation:: how
        /// </summary>
        measure_operation,

        /// <summary>
        /// The reporting category order: order of property categories. This is applicable only at class level
        /// </summary>
        reporting_categoryOrder,

        /// <summary>
        /// Indicating that this property is a String template, message contains wrapping template where {{{inner}}} is result of property value: template with placeholders {{{this_property_name}}} where this_ is automatically applied prefix
        /// </summary>
        reporting_template,

        /// <summary>
        /// The reporting valueformat <see cref="string"/>
        /// </summary>
        reporting_valueformat,

        /// <summary>
        /// Cell value will not be escaped by the renderer
        /// </summary>
        reporting_escapeoff,

        /// <summary>
        /// Property has special reporting function
        /// </summary>
        reporting_function,

        reporting_agregate_function,

        reporting_aggregation,

        /// <summary>
        /// The reporting column width: defines width of column in tabular report. Default value is: 20
        /// </summary>
        reporting_columnWidth,

        measure_letter,
        DataTableExport,

        /// <summary>
        /// The reporting hide: it is not shown on normal tables
        /// </summary>
        reporting_hide,

        ///// <summary>
        ///// The reporting presentation - adds measure to presentation
        ///// </summary>

        //reporting_presentation,
    }
}
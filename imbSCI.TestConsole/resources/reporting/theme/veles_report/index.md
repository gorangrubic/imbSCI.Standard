# imbVeles Experiment report

This is automatically generated report after the experiment test **`{{{test_runstamp}}}`** run finished after `{{{sys_runtime}}}`.

The most important sections:

+	Process reports: _reports on web spider and content analysis poperations_
+	Test: _description of the test performed_

At this stage only **`BlindSpider`** is implemented.

---

#  

 > typical HTML-based vs Semanticaly enhanced web spider algorithms

This experiment runs four different web spider algoritms against set of production company web sites. 
	

## Web Spider algorithms

All algorithms use basic language dictionary (Hunspell spell-check dictionary for Serbian language) to support language-oriented link evaluation.


#### Goal for spiders

 1. To bypass intro, language/region selection pages
 1. To detect a set of _the most important web pages_ **`IWP set`** within the web sites. Resultsing set should have reasonable size (from 4 to 10).
 1. In the **`IWP set`** it should have the following Key Pages with content/kind:

 - [x] About us
 - [x] Contacts
 - [x] Products
 - [x] Services

--- 
	
### BlindSpider

_Plain link ranking based on simple HTML links extraction_

#### Key features

+	URL tree-structure analysis to promote diversity in primary link sample
+	URL vs link title word match, Hunspell dictionary match
+	Page score affected by number of inbound, outbound and cross-bound links
+	Pages with score above _treshold_ are feed into IWP set

--- 
	
### TemplateSpider

_Extracts template and focuses on links within_

#### Forkflow

+	Basic link crawling pattern to collect **`PrimPage Set`** 
+	Runs [RB-UMTD] template detection over **`PrimPage Set`** 
+	Pages linked from the Template are feed into **`IWP set`**


#### Key features

+	Inherits link ranking algorithm [BlindSpider](#blindspider) 


--- 

### BasicSemanticSpider

_Content Semantic analysis of the first page_

Performs home page tokenzation, basic semantic analysis and extracts blocks

#### Tokenized page structure 

<div class="mermaid" id="monotone">

    graph TD
    linkStyle default interpolate monotone fill:none;
	C["Content"]
	B1["Block 1"]
	Bn["Block n"]
	P1["Paragraph 1"]
	Pn["Paragraph n"]
	S1_1["Sentence"]
	T1_1_1["Token"]
    C --> B1
	C --> Bn
	C -.-> B["..."]
	B1 --> P1
	B1 --> Pn
	B1 -.-> P["..."]
	P1 --> S1_1
	P1 --> S1_n
	P1 -.-> S["..."]
	S1_1 --> T1_1_1
	S1_1 --> T1_1_2
	S1_1 --> T1_1_3
	S1_1 --> T1_1_4
    S1_1 --> Tn
	S1_1 -.-> T["..."]

</div>

 > Tokenization brakes [C] Content into [B] blocks, [P] paragraphs, [S] sentences and [T] tokens.
 > Compresses HTML document structure _depth_ into flat content with minimal hierarchy

#### Forkflow

*	After semantic analysis picks a block with the most important navigation links 

#### Key features

*	Utilizes text-html hybrid tokenization algorihm, keeping record of all meta information from HTML
*	In case of proper home page it needs only one url request


--- 


### DomainSemanticSpider

_Domain-knowledge and Lexicon guided web analysis_

#### Key features

 *	Link and page evaluation is supported with **`bag-of-concept`** based on domain-knowledge
 *	[B] block and other content elements evaluation with extended langugage resources: Lexicon, expanded Dictionary


--- 

#### Technical note

Spider algorithm behaviour is defined by a class derived from `spiderEvaluatorBase`.



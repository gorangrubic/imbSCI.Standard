<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" 
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt" 
                xmlns="http://www.w3.org/2000/svg"
                exclude-result-prefixes="msxsl"
>
    <xsl:output method="html" 
                indent="no" 
                encoding="utf-8" 
                version="1.0" 
                doctype-system="http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd" 
                doctype-public="-//W3C//DTD SVG 1.1//EN" 
                omit-xml-declaration="no"
                media-type="image/svg+xml"
                xmlns="http://www.w3.org/2000/svg"
                />


    <xsl:template match="svg">

    <svg>

      <xsl:attribute name="width">
        <xsl:value-of select="svgDetails/width/."/>
      </xsl:attribute>
      <xsl:attribute name="height">
        <xsl:value-of select="svgDetails/height/."/>
      </xsl:attribute>

      <xsl:for-each select="//svgObjects/*">
        <xsl:choose>
          <xsl:when test="name()='rect'">
            <xsl:call-template name="rectangle"/>
          </xsl:when>
          <xsl:when test="name()='text'">
            <xsl:call-template name="text" />
          </xsl:when>
          <xsl:when test="name()='polygon'">
            <xsl:call-template name="polygon" />
          </xsl:when>
          <xsl:when test="name()='line'">
            <xsl:call-template name="line" />
          </xsl:when>
        </xsl:choose>
      </xsl:for-each>
      
    </svg>
    
  </xsl:template>

  <xsl:template name="rectangle">
    <rect fill="{@color}" x="{@x}" y="{@y}" width="{@width}" height="{@height}">
      <xsl:if test="@border='true'">
        <xsl:attribute name="stroke">Black</xsl:attribute>
        <xsl:attribute name="stroke-width">1</xsl:attribute>        
      </xsl:if>
    </rect>
  </xsl:template>

  <xsl:template name="text">
    <text x="{@x}" y="{@y}" font-size="{@font-size}" rotate="{@rotate}">
      <xsl:value-of select="."/>
    </text>
  </xsl:template>

  <xsl:template name="polygon">
    <polygon fill="{@color}" points="{@points}">
      <xsl:if test="@border='true'">
        <xsl:attribute name="stroke">Black</xsl:attribute>
        <xsl:attribute name="stroke-width">1</xsl:attribute>
      </xsl:if>
    </polygon>
  </xsl:template>

  <xsl:template name="line">
    <line fill="White" stroke="{@color}" stroke-width="{@width}" x1="{@x1}" y1="{@y1}" x2="{@x2}" y2="{@y2}">
      <xsl:if test="@dashed='true'">
        <xsl:attribute name="stroke-dasharray">4</xsl:attribute>
      </xsl:if>
    </line>
  </xsl:template>
  
  
</xsl:stylesheet>

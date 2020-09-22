<xsl:stylesheet xmlns:xsl = "http://www.w3.org/1999/XSL/Transform" version = "1.0">
<xsl:output omit-xml-declaration="no" method="xml" doctype-public="-//W3C//DTD XHTML 1.0 Strict//EN" doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd" indent="yes" encoding="UTF-8" />
<xsl:template match = "/icestats">
<pre>h</pre>

	<xsl:for-each select="source">
<xsl:text>&#xa;</xsl:text>
<h42><xsl:if test="artist"><xsl:value-of select="artist" /> - </xsl:if><xsl:value-of select="title" /></h42>
<xsl:text>&#xa;</xsl:text>
	</xsl:for-each>

<xsl:text>&#xa;</xsl:text>
<h42>(icecast source not connected)</h42>
<xsl:text>&#xa;</xsl:text>

</xsl:template>
</xsl:stylesheet>

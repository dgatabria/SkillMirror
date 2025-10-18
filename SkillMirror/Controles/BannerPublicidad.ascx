<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BannerPublicidad.ascx.cs" Inherits="SkillMirror.Controles.BannerPublicidad" %>

<asp:Panel ID="pnlCarrusel" runat="server" Visible="false">
    <div id="carouselPublicidad" class="carousel slide" data-bs-ride="carousel">
        
        <div class="carousel-indicators">
            <asp:Repeater ID="rptIndicadores" runat="server">
                <ItemTemplate>
                    <button type="button" data-bs-target="#carouselPublicidad" data-bs-slide-to="<%# Container.ItemIndex %>" class="<%# Container.ItemIndex == 0 ? "active" : "" %>"></button>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <div class="carousel-inner">
            <asp:Repeater ID="rptBanners" runat="server">
                <ItemTemplate>
                    <div class="carousel-item <%# Container.ItemIndex == 0 ? "active" : "" %>">
                        <a href="<%# Eval("URLDestino") %>" target="_blank" title="<%# Eval("Titulo") %>">
                            <img src="<%# Page.ResolveUrl(Eval("RutaImagen").ToString()) %>" class="d-block w-100" alt="<%# Eval("Titulo") %>">
                        </a>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <button class="carousel-control-prev" type="button" data-bs-target="#carouselPublicidad" data-bs-slide="prev">
            <span class="carousel-control-prev-icon" aria-hidden="true"></span>
            <span class="visually-hidden">Previous</span>
        </button>
        <button class="carousel-control-next" type="button" data-bs-target="#carouselPublicidad" data-bs-slide="next">
            <span class="carousel-control-next-icon" aria-hidden="true"></span>
            <span class="visually-hidden">Next</span>
        </button>
    </div>
</asp:Panel>